using System.Collections.Generic;

namespace Rusty.Graphs;

/// <summary>
/// A generic graph.
/// </summary>
public class Graph : IGraph
{
    /* Public properties. */
    public INodeData DefaultData { get; set; } = new NodeData();
    public int NodeCount => Nodes.Count;

    /* Private properties. */
    private List<IRootNode> Nodes { get; } = new();
    private Dictionary<IRootNode, int> Lookup { get; set; }

    /* Public methods. */
    public override string ToString()
    {
        return Serializer.ToString(this);
    }

    /// <summary>
    /// Check if this graph contains a node.
    /// </summary>
    public bool ContainsNode(IRootNode node)
    {
        EnsureLookup();
        return Lookup.ContainsKey(node);
    }

    /// <summary>
    /// Return the index of a node.
    /// </summary>
    public int IndexOfNode(IRootNode node)
    {
        EnsureLookup();
        try
        {
            return Lookup[node];
        }
        catch
        {
            return -1;
        }
    }

    /// <summary>
    /// Get a node from the graph, using its index.
    /// </summary>
    public virtual IRootNode GetNodeAt(int index)
    {
        return Nodes[index];
    }

    /// <summary>
    /// Create a new node, add it to the graph, and return the node.
    /// </summary>
    public virtual IRootNode CreateNode()
    {
        // Create a node.
        RootNode node = new();
        node.Data = DefaultData.Copy();

        // Add the node.
        AddNode(node);

        // Return the result.
        return node;
    }

    /// <summary>
    /// Add a node to this graph. This removes it from its old graph, if it was contained in one.
    /// </summary>
    public void AddNode(IRootNode node)
    {
        if (node == null)
            return;

        EnsureLookup();

        // Remove the node from its old graph.
        node.Remove();

        // Add the node to this graph.
        Lookup.Add(node, Nodes.Count);
        Nodes.Add(node);
        node.Graph = this;
    }

    /// <summary>
    /// Insert a node into the graph.
    /// </summary>
    public void InsertNode(int index, IRootNode node)
    {
        Nodes.Insert(index, node);
        Lookup = null;
    }

    /// <summary>
    /// Remove a node from the graph.
    /// </summary>
    public void RemoveNode(IRootNode node)
    {
        Nodes.Remove(node);
        node.Graph = null;
        node.ClearInputs();
        node.ClearOutputs();
        Lookup = null;
    }

    /// <summary>
    /// Remove all nodes from the graph.
    /// </summary>
    public void ClearNodes()
    {
        Nodes.Clear();
        Lookup = null;
    }

    /// <summary>
    /// Find and return the indices of all start nodes in the graph. This will include all nodes without a precursor. Cycles
    /// with no clear start node will arbitrarily get one of their nodes marked as a start node.
    /// </summary>
    public int[] FindStartNodes()
    {
        return StartNodeFinder.FindStartNodes(this);
    }

    /* Private methods. */
    private void EnsureLookup()
    {
        if (Lookup != null)
            return;

        Lookup = new();
        for (int i = 0; i < Nodes.Count; i++)
        {
            Lookup.Add(Nodes[i], i);
        }
    }
}