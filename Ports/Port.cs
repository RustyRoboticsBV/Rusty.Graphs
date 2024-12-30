using System.Collections.Generic;

namespace Rusty.Graphs
{
    /// <summary>
    /// A top-level node. Can have inputs and outputs, and can contain sub-nodes.
    /// </summary>
    public abstract class Port<DataT>
        where DataT : new()
    {
        /* Public properties. */
        public RootNode<DataT> Owner { get; internal set; }
        public List<(string, object)> Attributes { get; } = new();

        /* Constructors. */
        public Port() : base() { }

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