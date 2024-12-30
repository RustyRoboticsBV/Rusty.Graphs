using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rusty.Graphs
{
    /// <summary>
    /// A top-level node. Can have inputs and outputs, and can contain sub-nodes.
    /// </summary>
    public class RootNode<DataT> : Node<DataT>
        where DataT : new()
    {
        /* Public properties. */
        public List<InputPort<DataT>> Inputs { get; } = new();
        public List<OutputPort<DataT>> Outputs { get; } = new();

        /* Constructors. */
        public RootNode() : base() { }

        public RootNode(DataT data) : base(data) { }

        /* Casting operators. */
        public static implicit operator SubNode<DataT>(RootNode<DataT> rootNode)
        {
            return new(rootNode.Data, rootNode.Children);
        }

        /* Public methods. */
        public override void Remove()
        {
            foreach (var input in Inputs)
            {
                input.Remove();
            }
            foreach (var output in Outputs)
            {
                output.Remove();
            }
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

        public override void AddChild(RootNode<DataT> rootNode)
        {
            base.AddChild(rootNode);

            foreach (var input in rootNode.Inputs)
            {
                input.Owner = this;
            }
            foreach (var output in rootNode.Outputs)
            {
                output.Owner = this;
            }
        }

        public void ConnectTo(int outputPortIndex, RootNode<DataT> toNode, int inputPortIndex)
        {
            // Ensure output & input ports.
            while (Outputs.Count <= outputPortIndex)
            {
                Outputs.Add(new());
            }
            while (toNode.Inputs.Count <= inputPortIndex)
            {
                toNode.Inputs.Add(new());
            }

            // Connect ports.
            Outputs[outputPortIndex].ConnectTo(toNode.Inputs[inputPortIndex]);
        }

        public void ConnectTo(RootNode<DataT> toNode)
        {
            OutputPort<DataT> output = new();
            Outputs.Add(output);
            InputPort<DataT> input = new();
            toNode.Inputs.Add(input);
            output.ConnectTo(input);
        }
    }
}