using System.Collections.Generic;

namespace Rusty.Graphs
{
    /// <summary>
    /// A node that is a direct child of a graph. Can have inputs and outputs, and can contain sub-nodes, but cannot be contained
    /// within a node itself.
    /// </summary>
    public class RootNode<DataT> : Node<DataT>
        where DataT : new()
    {
        /* Public properties. */
        public override Graph<DataT> Graph { get; internal set; }
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
            // Remove the input and output ports.
            foreach (var input in Inputs)
            {
                input.Remove();
            }
            foreach (var output in Outputs)
            {
                output.Remove();
            }

            // Remove this node from its graph.
            if (Graph != null)
            {
                try
                {
                    Graph.Nodes.Remove(this);
                }
                catch { }
                Graph = null;
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

        /// <summary>
        /// Removes another root node from the graph, converts it to a sub-node and adds that node as a child. All inputs and
        /// output connections of the consumed node are transferred to us.
        /// </summary>
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

        /// <summary>
        /// Connect a specific output port to a specific input port of another node. If the port doesn't exist yet, it is
        /// created.
        /// </summary>
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

        /// <summary>
        /// Connect this node to another node. Creates new output and input ports.
        /// </summary>
        public void ConnectTo(RootNode<DataT> toNode)
        {
            // Create start port.
            OutputPort<DataT> output = new();
            Outputs.Add(output);

            // Create end port.
            InputPort<DataT> input = new();
            toNode.Inputs.Add(input);

            // Connect the two.
            output.ConnectTo(input);
        }
    }
}