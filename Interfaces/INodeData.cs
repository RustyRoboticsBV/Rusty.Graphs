namespace Rusty.Graphs;

/// <summary>
/// An interface for data that can be stored on graph nodes.
/// </summary>
public interface INodeData
{
    public string ToString();

    /// <summary>
    /// Make a shallow copy of this data object.
    /// </summary>
    public INodeData Copy();
}