namespace Rusty.Graphs
{
    /// <summary>
    /// A top-level node. Can have inputs and outputs, and can contain sub-nodes.
    /// </summary>
    public class OutputPort<DataT> : Port<DataT>
        where DataT : NodeData, new()
    {
        /* Public properties. */
        /// <summary>
        /// The target input port that this output is connected to.
        /// </summary>
        public InputPort<DataT> To { get; internal set; }
        /// <summary>
        /// The node to whose input port this output port is connected to.
        /// </summary>
        public RootNode<DataT> ToNode => To != null ? To.Owner : null;
        /// <summary>
        /// The index of the target input port that this output is connected to, inside its owner node's list of inputs.
        /// </summary>
        public int ToPortIndex => ToNode != null ? ToNode.Inputs.IndexOf(To) : -1;

        /* Constructors. */
        public OutputPort() : base() { }

        /* Public methods. */
        public sealed override void Remove()
        {
            Disconnect();
            if (Owner != null)
                Owner.Outputs.Remove(this);
        }

        public sealed override void Disconnect()
        {
            if (To != null)
            {
                To.From = null;
                To = null;
            }
        }

        /// <summary>
        /// Connect this output port to an input port. Does nothing if the port has already been connected.
        /// </summary>
        public void ConnectTo(InputPort<DataT> inputPort)
        {
            Disconnect();

            To = inputPort;
            if (inputPort != null)
                inputPort.From = this;
        }

        /// <summary>
        /// Connect this output port to a newly-created input port on the target node. Does nothing if the output has already
        /// been connected to this node.
        /// </summary>
        public void ConnectTo(RootNode<DataT> toNode)
        {
            if (To != null && To.Owner == toNode)
                return;

            InputPort<DataT> input = new InputPort<DataT>();
            input.Owner = toNode;
            toNode.Inputs.Add(input);

            ConnectTo(input);
        }
    }
}