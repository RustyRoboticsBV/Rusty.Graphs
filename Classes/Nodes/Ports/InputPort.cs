namespace Rusty.Graphs;

/// <summary>
/// A top-level node. Can have inputs and outputs, and can contain sub-nodes.
/// </summary>
public class InputPort : Port, IInputPort
{
    /* Public properties. */
    /// <summary>
    /// The source output port that this input is connected to.
    /// </summary>
    public IOutputPort From { get; set; }

    /* Constructors. */
    public InputPort() : base() { }

    /* Public methods. */
    public sealed override void Remove()
    {
        Disconnect();
        Node?.RemoveInput(this);
    }

    public sealed override void Disconnect()
    {
        From?.Disconnect();
    }

    public void ConnectTo(IOutputPort output)
    {
        Disconnect();
        From = output;
    }
}