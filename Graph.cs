using System.Collections.Generic;

namespace Rusty.Graphs
{
    /// <summary>
    /// A container for nodes.
    /// </summary>
    public class Graph<DataT> where DataT : new()
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
                str += Nodes[i];
            }
            return str;
        }

        /// <summary>
        /// Create a new node, add it to the graph, and return the node.
        /// </summary>
        public RootNode<DataT> AddNode()
        {
            return AddNode("");
        }

        /// <summary>
        /// Create a new node, add it to the graph, and return the node.
        /// </summary>
        public RootNode<DataT> AddNode(string name)
        {
            return AddNode(name, new());
        }

        public RootNode<DataT> AddNode(string name, DataT data)
        {
            // Create a new node.
            RootNode<DataT> newNode = new(name, data);

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
            if (node == null)
                return;

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

        /// <summary>
        /// Transfer all the nodes of another graph to this one.
        /// </summary>
        public void Consume(Graph<DataT> other)
        {
            for (int i = 0; i < other.Nodes.Count; i++)
            {
                other.Nodes[i].Graph = this;
            }
            other.Nodes.Clear();
        }
    }
}