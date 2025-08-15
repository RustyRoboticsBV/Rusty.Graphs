using System.Collections.Generic;

namespace Rusty.Graphs;

/// <summary>
/// A node that is a direct child of a graph. Can have inputs and outputs, and can contain sub-nodes, but cannot be contained
/// within a node itself.
/// </summary>
public class RootNode : Node, IRootNode
{
    /* Public properties. */
    public int InputCount => Inputs.Count;
    public int OutputCount => Outputs.Count;
    public override IGraph Graph { get; set; }

    /* Private properties */
    private List<IInputPort> Inputs { get; } = new();
    private List<IOutputPort> Outputs { get; } = new();

    /* Public methods. */
    public override string ToString()
    {
        return Serializer.ToString(this);
    }


    public bool ContainsInput(IInputPort input)
    {
        return Inputs.Contains(input);
    }

    public int GetInputIndex(IInputPort input)
    {
        return Inputs.IndexOf(input);
    }

    public IInputPort GetInputAt(int index)
    {
        return Inputs[index];
    }

    public virtual IInputPort CreateInput()
    {
        InputPort input = new();
        AddInput(input);
        return input;
    }

    public void AddInput(IInputPort input)
    {
        Inputs.Add(input);
        input.Node = this;
    }

    public void InsertInput(int index, IInputPort input)
    {
        Inputs.Insert(index, input);
        input.Node = this;
    }

    public void RemoveInput(IInputPort input)
    {
        Inputs.Remove(input);
        input.Node = null;
    }

    public void ClearInputs()
    {
        while (InputCount > 0)
        {
            RemoveInput(GetInputAt(0));
        }
    }


    public bool ContainsOutput(IOutputPort output)
    {
        return Outputs.Contains(output);
    }

    public int GetOutputIndex(IOutputPort output)
    {
        return Outputs.IndexOf(output);
    }

    public IOutputPort GetOutputAt(int index)
    {
        return Outputs[index];
    }

    public virtual IOutputPort CreateOutput()
    {
        OutputPort output = new();
        AddOutput(output);
        return output;
    }

    public void AddOutput(IOutputPort output)
    {
        Outputs.Add(output);
        output.Node = this;
    }

    public void InsertOutput(int index, IOutputPort output)
    {
        Outputs.Insert(index, output);
        output.Node = this;
    }

    public void RemoveOutput(IOutputPort output)
    {
        Outputs.Remove(output);
        output.Node = null;
    }

    public void ClearOutputs()
    {
        while (OutputCount > 0)
        {
            RemoveOutput(GetOutputAt(0));
        }
    }


    public virtual ISubNode ToChild()
    {
        SubNode child = new();
        child.Data = Data.Copy();
        return child;
    }


    public override void Remove()
    {
        ClearInputs();
        ClearOutputs();
        if (Graph != null && Graph.ContainsNode(this))
            Graph.RemoveNode(this);
    }

    public override void Dissolve()
    {
        for (int i = 0; i < Inputs.Count && i < Outputs.Count; i++)
        {
            if (Inputs[i].From != null)
                Inputs[i].From.ConnectTo(Outputs[i].To);
        }
        Remove();
    }

    /// <summary>
    /// Connect a specific output port to a specific input port of another node. If the port doesn't exist yet, it is
    /// created.
    /// </summary>
    public void ConnectTo(int outputPortIndex, RootNode toNode, int inputPortIndex)
    {
        // Ensure output & input ports.
        while (Outputs.Count <= outputPortIndex)
        {
            CreateOutput();
        }
        while (toNode.Inputs.Count <= inputPortIndex)
        {
            toNode.CreateInput();
        }

        // Connect ports.
        GetOutputAt(outputPortIndex).ConnectTo(toNode.GetInputAt(inputPortIndex));
    }

    /// <summary>
    /// Connect this node to another node. Creates new output and input ports.
    /// </summary>
    public void ConnectTo(RootNode toNode)
    {
        IOutputPort output = CreateOutput();
        if (toNode != null)
        {
            IInputPort input = toNode.CreateInput();
            output.ConnectTo(input);
        }
    }
}