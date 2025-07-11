namespace Rusty.Graphs
{
    /// <summary>
    /// A top-level node. Can have inputs and outputs, and can contain sub-nodes.
    /// </summary>
    public abstract class Port : IPort
    {
        /* Public properties. */
        public IRootNode Node { get; set; }

        /* Constructors. */
        public Port() { }

        /* Public methods. */
        /// <summary>
        /// Remove this port from its owner node.
        /// </summary>
        public abstract void Remove();

        /// <summary>
        /// Disconnect this port from the port it's connected to.
        /// </summary>
        public abstract void Disconnect();
    }
}