using System.Collections.Generic;

namespace Rusty.Graphs
{
    /// <summary>
    /// A container for nodes.
    /// </summary>
    public abstract class Graph<DataT> where DataT : new()
    {
        /* Public properties. */
        public string Name { get; set; }
        public List<RootNode<DataT>> Nodes { get; } = new();

        /* Public methods. */
        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (i > 0)
                    str += "\n";
                str += ToString(Nodes[i]);
            }
            return str;
        }

        /// <summary>
        /// Create a new node, add it to the graph, and return the node.
        /// </summary>
        public RootNode<DataT> CreateNode()
        {
            // Create a new node.
            RootNode<DataT> newNode = new();

            // Add the node.
            AddNode(newNode);

            // Return the result.
            return newNode;
        }

        /// <summary>
        /// Add a node to this graph. This removes it from its old graph, if it was contained in one.
        /// </summary>
        public void AddNode(RootNode<DataT> node)
        {
            // Remove the node from its old graph.
            node.Remove();

            // Add the node to this graph.
            Nodes.Add(node);
            node.Graph = this;
        }

        /// <summary>
        /// Remove a node from the graph.
        /// </summary>
        public void RemoveNode(RootNode<DataT> node)
        {
            node.Remove();
        }

        /// <summary>
        /// Replace a node in the graph.
        /// </summary>
        public void ReplaceNode(RootNode<DataT> oldNode, RootNode<DataT> newNode)
        {
            // Transfer input/output ports.
            while (oldNode.Inputs.Count > 0)
            {
                InputPort<DataT> input = oldNode.Inputs[0];
                oldNode.Inputs.RemoveAt(0);

                newNode.Inputs.Add(input);
                input.Owner = newNode;
            }
            while (oldNode.Outputs.Count > 0)
            {
                OutputPort<DataT> output = oldNode.Outputs[0];
                oldNode.Outputs.RemoveAt(0);

                newNode.Outputs.Add(output);
                output.Owner = newNode;
            }

            // Replace node in graph.
            int index = Nodes.IndexOf(oldNode);
            if (index != -1)
                Nodes[index] = newNode;
            newNode.Graph = this;
            oldNode.Graph = null;
        }

        /* Private methods. */
        private static string ToString(RootNode<DataT> node, HashSet<Node<DataT>> examined = null)
        {
            if (examined == null)
                examined = new();

            // Detect cycles.
            if (examined.Contains(node))
                return $"({node.Name})";

            // Base stringify.
            string str = ToString(node as Node<DataT>);

            // Enclose in borders.
            int width = GetWidth(str);
            str = '|' + str.Replace("\n", "│\n│") + '|';
            str = '┌' + new string('─', width) + '┐';
            str = '└' + new string('─', width) + '┘';

            // Add title line.
            if (node.Children.Count > 0)
                str.Insert(str.IndexOf('\n') + 1, '├' + new string('─', GetWidth(str)) + '┤');

            return str;
        }

        private static string ToString(Node<DataT> node, HashSet<Node<DataT>> examined = null)
        {
            // Detect cycles.
            if (examined.Contains(node))
                return $"({node.Name})";
            examined.Add(node);

            // Stringify node.
            string str = node.Name;

            // Stringify children.
            for (int i = 0; i < node.Children.Count; i++)
            {
                string childStr = ToString(node.Children[i]);

                if (i == node.Children.Count - 1)
                    childStr = '└' + childStr;
                else
                    childStr = '├' + childStr;

                if (i < node.Children.Count - 2)
                    childStr = childStr.Replace("\n", "\n│");

                str += '\n' + childStr;
            }

            return str;
        }

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

        private static int GetHeight(string str)
        {
            int count = 0;
            for (int i = 0; i < str.Length; i++)
                if (str[i] == '\n')
                    count++;
            return count;
        }
    }
}