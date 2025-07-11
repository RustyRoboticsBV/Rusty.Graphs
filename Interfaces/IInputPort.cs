namespace Rusty.Graphs;

/// <summary>
/// An interface for root node input ports.
/// </summary>
public interface IInputPort : IPort
{
    /* Public properties. */
    /// <summary>
    /// The source output port that this input is connected to.
    /// </summary>
    public IOutputPort From { get; set; }

    /* Public methods. */
    /// <summary>
    /// Connect this input port to an output port. If either port was already connected, they get disconnected first.
    /// </summary>
    public void ConnectTo(IOutputPort outputPort);
}