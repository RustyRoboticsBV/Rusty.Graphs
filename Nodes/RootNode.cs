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

        public RootNode(string name) : base(name) { }

        public RootNode(string name, DataT data) : base(name, data) { }

        /* Casting operators. */
        public static implicit operator SubNode<DataT>(RootNode<DataT> rootNode)
        {
            return new(rootNode.Name, rootNode.Data, rootNode.Children);
        }

        /* Public methods. */
        public override string ToString(HashSet<Node<DataT>> examined)
        {
            string str = "";
            int width = 0;

            // Detect cycles.
            bool examinedAlready = examined.Contains(this);
            if (examinedAlready)
            {
                str = $"({Name}...)";
                width = str.Length;
            }

            else
            {
                // Base stringify.
                str = base.ToString(examined);

                // Make every line the same width.
                width = GetWidth(str);
                string[] substrs = str.Split('\n');
                str = "";
                for (int i = 0; i < substrs.Length; i++)
                {
                    substrs[i] += new string(' ', width - substrs[i].Length);
                    if (i > 0)
                        str += '\n';
                    str += substrs[i];
                }
            }

            // Enclose in borders.
            str = '│' + str.Replace("\n", "│\n│") + '│';
            str = '┌' + new string('─', width) + '┐'
                + $"\n{str}\n"
                + '└' + new string('─', width) + '┘';

            // Stop here if we already examined this node.
            if (examinedAlready)
                return str;

            // Add title line.
            if (Children.Count > 0)
                str = str.Insert(width * 2 + 6, '├' + new string('─', width) + '┤' + '\n');

            // Handle outputs.
            if (Outputs.Count > 0)
                str = str.Replace("└─", "└┬");
            for (int i = 0; i < Outputs.Count; i++)
            {
                InputPort<DataT> to = Outputs[i].To;
                string childStr = "";
                if (to == null || to.Owner == null)
                    childStr = "(null)";
                else
                    childStr = Outputs[i].To.Owner.ToString(examined);

                if (i < Outputs.Count - 1)
                {
                    childStr = ReplaceFirst(childStr, "\n│", "\n├┤");
                    childStr = childStr.Replace("│\n", "│\n│");
                }
                else
                {
                    childStr = ReplaceFirst(childStr, "│", "└┤");
                    childStr = childStr.Replace("│\n", "│\n ");
                }
                childStr = " │" + childStr.Replace("\n", "\n ");

                str += '\n' + childStr;
            }

            return str;
        }

        public override void Remove()
        {
            // Remove the input and output ports.
            for (int i = 0; i < Inputs.Count; i++)
            {
                Inputs[i].Remove();
            }
            for (int i = 0; i < Outputs.Count; i++)
            {
                Outputs[i].Remove();
            }

            // Remove this node from its graph.
            if (Graph != null && Graph.Nodes.Contains(this)))
                Graph.Nodes.Remove(this);
            Graph = null;
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
        public override void Consume(RootNode<DataT> node)
        {
            if (node == null)
                return;

            for (int i = 0; i < node.Inputs.Count; i++)
            {
                Inputs.Add(node.Inputs[i]);
                node.Inputs[i].Owner = this;
            }
            node.Inputs.Clear();
            for (int i = 0; i < node.Outputs.Count; i++)
            {
                Outputs.Add(node.Outputs[i]);
                node.Outputs[i].Owner = this;
            }
            node.Outputs.Clear();

            base.Consume(node);
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
            output.Owner = this;

            // Create end port.
            InputPort<DataT> input = new();
            toNode.Inputs.Add(input);
            input.Owner = toNode;

            // Connect the two.
            output.ConnectTo(input);
        }

        /* Private methods. */
        /// <summary>
        /// Get the width of a string (which potentially contains line-breaks).
        /// </summary>
        private static int GetWidth(string str)
        {
            if (!str.Contains('\n'))
                return str.Length;

            string[] substrs = str.Split('\n');
            int width = 0;
            foreach (string substr in substrs)
            {
                if (substr.Length > width)
                    width = substr.Length;
            }
            return width;
        }

        /// <summary>
        /// Replace the first occurance of a substring within a string.
        /// </summary>
        public string ReplaceFirst(string str, string oldValue, string newValue)
        {
            int pos = str.IndexOf(oldValue);
            if (pos < 0)
                return str;
            return str.Substring(0, pos) + newValue + str.Substring(pos + oldValue.Length);
        }
    }
}