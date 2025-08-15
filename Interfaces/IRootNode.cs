namespace Rusty.Graphs;

/// <summary>
/// A node that is a direct child of a graph. Can have inputs and outputs, and can contain sub-nodes, but cannot be contained
/// within a node itself.
/// </summary>
public interface IRootNode : INode
{
    /* Public properties. */
    /// <summary>
    /// The number of input ports on this node.
    /// </summary>
    public int InputCount { get; }
    /// <summary>
    /// The number of output ports on this node.
    /// </summary>
    public int OutputCount { get; }

    /* Public methods. */
    /// <summary>
    /// Check whether or not this node contains some input port.
    /// </summary>
    public bool ContainsInput(IInputPort input);
    /// <summary>
    /// Get the index of an input port.
    /// </summary>
    public int GetInputIndex(IInputPort input);
    /// <summary>
    /// Get an input port.
    /// </summary>
    public IInputPort GetInputAt(int index);
    /// <summary>
    /// Create a new input port.
    /// </summary>
    public IInputPort CreateInput();
    /// <summary>
    /// Add an input port. This removes it from its old node, if it had one.
    /// </summary>
    public void AddInput(IInputPort input);
    /// <summary>
    /// Insert an input port. This removes it from its old node, if it had one.
    /// </summary>
    public void InsertInput(int index, IInputPort input);
    /// <summary>
    /// Remove an input.
    /// </summary>
    public void RemoveInput(IInputPort index);
    /// <summary>
    /// Disconnect and remove all inputs.
    /// </summary>
    public void ClearInputs();

    /// <summary>
    /// Check whether or not this node contains some output port.
    /// </summary>
    public bool ContainsOutput(IOutputPort output);
    /// <summary>
    /// Get the index of an output port.
    /// </summary>
    public int GetOutputIndex(IOutputPort output);
    /// <summary>
    /// Get an output port.
    /// </summary>
    public IOutputPort GetOutputAt(int index);
    /// <summary>
    /// Create a new output port.
    /// </summary>
    public IOutputPort CreateOutput();
    /// <summary>
    /// Add an output port. This removes it from its old node, if it had one.
    /// </summary>
    public void AddOutput(IOutputPort output);
    /// <summary>
    /// Insert an output port. This removes it from its old node, if it had one.
    /// </summary>
    public void InsertOutput(int index, IOutputPort output);
    /// <summary>
    /// Remove an output.
    /// </summary>
    public void RemoveOutput(IOutputPort index);
    /// <summary>
    /// Disconnect and remove all outputs.
    /// </summary>
    public void ClearOutputs();

    /// <summary>
    /// Create a sub-node that contains a copy of this root node's data. This does NOT copy child nodes, inputs or outputs.
    /// </summary>
    public ISubNode ToChild();
}