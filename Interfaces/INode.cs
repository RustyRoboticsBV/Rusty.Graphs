namespace Rusty.Graphs;

/// <summary>
/// The base class for all nodes.
/// </summary>
public interface INode
{
    /* Public properties. */
    /// <summary>
    /// The data contained on this node.
    /// </summary>
    public INodeData Data { get; set; }
    /// <summary>
    /// The graph that this node is contained on.
    /// </summary>
    public IGraph Graph { get; set; }
    /// <summary>
    /// The number of child nodes that this node has.
    /// </summary>
    public int ChildCount { get; }

    /* Public methods. */
    /// <summary>
    /// Check whether or not this node has some child node.
    /// </summary>
    public bool ContainsChild(ISubNode child);
    /// <summary>
    /// Get the index of a child node.
    /// </summary>
    public int GetChildIndex(ISubNode child);
    /// <summary>
    /// Get the child node with some index.
    /// </summary>
    public ISubNode GetChildAt(int index);
    /// <summary>
    /// Create a child node, add it to this node, and return it.
    /// </summary>
    public ISubNode CreateChild();
    /// <summary>
    /// Add a child node. This removes the node from its old parent, if it had one.
    /// </summary>
    public void AddChild(INode node);
    /// <summary>
    /// Insert a child node. This removes the node from its old parent, if it had one.
    /// </summary>
    public void InsertChild(int index, INode node);
    /// <summary>
    /// Replace a child node.
    /// </summary>
    public void ReplaceChild(ISubNode oldChild, INode newChild);
    /// <summary>
    /// Move a child node to a new index.
    /// </summary>
    public void MoveChild(ISubNode child, int newIndex);
    /// <summary>
    /// Remove a child node from this node.
    /// </summary>
    public void RemoveChild(ISubNode child);
    /// <summary>
    /// Remove all child nodes from this node.
    /// </summary>
    public void ClearChildren();
    /// <summary>
    /// Transfer the children form another node to this node.
    /// </summary>
    public void StealChildren(INode from);

    /// <summary>
    /// Remove this node from its graph or parent node, removing all of its connections in the process.
    /// </summary>
    public void Remove();
    /// <summary>
    /// Removes the node from its graph or parent node, while preserving transitive connections where possible.
    /// </summary>
    public void Dissolve();
}