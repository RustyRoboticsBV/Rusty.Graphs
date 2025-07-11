namespace Rusty.Graphs
{
    /// <summary>
    /// A top-level node. Can have inputs and outputs, and can contain sub-nodes.
    /// </summary>
    public class OutputPort : Port, IOutputPort
    {
        /* Public properties. */
        public IInputPort To { get; set; }

        /* Public methods. */
        public sealed override void Remove()
        {
            Disconnect();
            Node?.RemoveOutput(this);
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
        public void ConnectTo(IInputPort inputPort)
        {
            Disconnect();

            To = inputPort;
            if (inputPort != null)
                inputPort.From = this;
        }

        /// <summary>
        /// Connect this output port to a newly-created input port on the target node.
        /// </summary>
        public void ConnectTo(IRootNode toNode)
        {
            Disconnect();
            IInputPort input = toNode.CreateInput();
            ConnectTo(input);
        }
    }
}