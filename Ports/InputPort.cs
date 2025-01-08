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
        /// <summary>
        /// The node to whose output port this input port is connected to.
        /// </summary>
        public RootNode<DataT> FromNode => From != null ? From.Owner : null;
        /// <summary>
        /// The index of the target output port that this input is connected to, inside its owner node's list of outputs.
        /// </summary>
        public int FromPortIndex => FromNode != null ? FromNode.Outputs.IndexOf(From) : -1;

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