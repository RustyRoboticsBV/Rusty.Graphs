namespace Rusty.Graphs
{
    /// <summary>
    /// A top-level node. Can have inputs and outputs, and can contain sub-nodes.
    /// </summary>
    public class InputPort<DataT> : Port<DataT>
        where DataT : new()
    {
        /* Public properties. */
        /// <summary>
        /// The source output port that this input is connected to.
        /// </summary>
        public OutputPort<DataT> From { get; internal set; }

        /* Constructors. */
        public InputPort() : base() { }

        /* Public methods. */
        public sealed override void Remove()
        {
            Owner.Inputs.Remove(this);
        }

        public sealed override void Disconnect()
        {
            if (From != null)
                From.Disconnect();
        }
    }
}