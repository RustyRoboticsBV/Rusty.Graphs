namespace Rusty.Graphs
{
    /// <summary>
    /// A top-level node. Can have inputs and outputs, and can contain sub-nodes.
    /// </summary>
    public class OutputPort<DataT> : Port<DataT>
        where DataT : new()
    {
        /* Public properties. */
        /// <summary>
        /// The target input port that this output is connected to.
        /// </summary>
        public InputPort<DataT> To { get; internal set; }

        /* Constructors. */
        public OutputPort() : base() { }

        /* Public methods. */
        public sealed override void Remove()
        {
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
    }
}