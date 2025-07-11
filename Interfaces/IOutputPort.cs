namespace Rusty.Graphs;

/// <summary>
/// An interface for root node output ports.
/// </summary>
public interface IOutputPort : IPort
{
    /* Public properties. */
    /// <summary>
    /// The target input port that this output is connected to.
    /// </summary>
    public IInputPort To { get; set; }

    /* Public methods. */
    /// <summary>
    /// Connect this output port to an input port. If either port was already connected, they get disconnected first.
    /// </summary>
    public void ConnectTo(IInputPort inputPort);
}