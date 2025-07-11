namespace Rusty.Graphs;

/// <summary>
/// A node that can be contained within another node. Has no inputs/outputs, but has data can have sub-nodes of its own.
/// </summary>
public interface ISubNode : INode
{
    /* Public properties. */
    /// <summary>
    /// The parent of this sub-node.
    /// </summary>
    public INode Parent { get; set; }
    /// <summary>
    /// The root of this node (which is a direct child of the graph).
    /// </summary>
    public IRootNode Root { get; }
}