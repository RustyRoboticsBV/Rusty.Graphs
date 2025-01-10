namespace Rusty.Graphs
{
    /// <summary>
    /// A block of data that can be stored inside of a graph, a root node or a sub-node.
    /// </summary>
    public abstract class NodeData
    {
        /* Public methods. */
        public sealed override string ToString()
        {
            return GetName();
        }

        /// <summary>
        /// Convert the node data object to a node name.
        /// </summary>
        public abstract string GetName();
    }
}