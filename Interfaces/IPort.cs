namespace Rusty.Graphs;

/// <summary>
/// An interface for root node ports.
/// </summary>
public interface IPort
{
    /* Public properties. */
    public IRootNode Node { get; internal set; }

    /* Public methods. */
    /// <summary>
    /// Remove this port from its owner node.
    /// </summary>
    public void Remove();

    /// <summary>
    /// Disconnect this port from the port it's connected to.
    /// </summary>
    public void Disconnect();
}