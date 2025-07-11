namespace Rusty.Graphs;

/// <summary>
/// An interface for graphs consisting of nodes.
/// </summary>
public interface IGraph
{
    /* Public properties. */
    /// <summary>
    /// The number of nodes contained on this graph.
    /// </summary>
    public int NodeCount { get; }

    /* Public methods. */
    /// <summary>
    /// Check if this graph contains a node.
    /// </summary>
    public bool ContainsNode(IRootNode node);
    /// <summary>
    /// Return the index of a node.
    /// </summary>
    public int IndexOfNode(IRootNode node);
    /// <summary>
    /// Get a node from the graph, using its index.
    /// </summary>
    public IRootNode GetNodeAt(int index);
    /// <summary>
    /// Create a new node, add it to the graph, and return the node.
    /// </summary>
    public IRootNode CreateNode();
    /// <summary>
    /// Add a node to this graph. This removes it from its old graph, if it was contained in one.
    /// </summary>
    public void AddNode(IRootNode node);
    /// <summary>
    /// Insert a node into the graph.
    /// </summary>
    public void InsertNode(int index, IRootNode node);
    /// <summary>
    /// Remove a node from the graph.
    /// </summary>
    public void RemoveNode(IRootNode node);
    /// <summary>
    /// Remove all nodes from the graph.
    /// </summary>
    public void ClearNodes();
}