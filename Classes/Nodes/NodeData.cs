namespace Rusty.Graphs
{
    /// <summary>
    /// A block of data that can be stored inside graph node.
    /// </summary>
    public class NodeData : INodeData
    {
        public virtual INodeData Copy()
        {
            return new NodeData();
        }
    }
}