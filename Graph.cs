using System.Collections.Generic;

namespace Rusty.Graphs
{
    /// <summary>
    /// A container for nodes.
    /// </summary>
    public class Graph<DataT> where DataT : NodeData, new()
    {
        /* Public properties. */
        public DataT Data { get; set; } = new();
        public int Count => Nodes.Count;

        /* Private properties. */
        private List<RootNode<DataT>> Nodes { get; } = new();
        private Dictionary<RootNode<DataT>, int> Lookup { get; } = new();

        /* Indexers. */
        public RootNode<DataT> this[int index] => Nodes[index];

        /* Public methods. */
        /// <summary>
        /// Check if this graph contains a node.
        /// </summary>
        public bool ContainsNode(RootNode<DataT> node)
        {
            return Lookup.ContainsKey(node);
        }

        /// <summary>
        /// Return the index of a node.
        /// </summary>
        public int IndexOfNode(RootNode<DataT> node)
        {
            try
            {
                return Lookup[node];
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Create a new node, add it to the graph, and return the node.
        /// </summary>
        public RootNode<DataT> AddNode()
        {
            // Create a node.
            RootNode<DataT> node = CreateNode(new());

            // Add the node.
            AddNode(node);

            // Return the result.
            return node;
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
            Lookup.Add(node, Nodes.Count);
            Nodes.Add(node);
            node.Graph = this;
        }

        /// <summary>
        /// Remove a node from the graph.
        /// </summary>
        public void RemoveNode(RootNode<DataT> node)
        {
            Lookup.Remove(node);
            Nodes.Remove(node);
            node.Graph = null;
        }

        /// <summary>
        /// Find and return the indices of all start nodes in the graph. This will include all nodes without a precursor. Cycles
        /// with no clear start node will arbitrarily get one of their nodes marked as a start node.
        /// </summary>
        public int[] FindStartNodes()
        {
            // Initialize start nodes list and visited array.
            List<int> startNodes = new List<int>();
            bool[] visited = new bool[Count];

            // Find non-cycle start nodes.
            for (int i = 0; i < Count; i++)
            {
                RootNode<DataT> node = Nodes[i];

                // Check if this node has at least one precursor node.
                bool hasPrecursor = false;
                for (int j = 0; j < node.Inputs.Count; j++)
                {
                    if (node.Inputs[j].FromNode != null)
                    {
                        hasPrecursor = true;
                        break;
                    }
                }

                // If we had no precursors, we are a start node.
                if (!hasPrecursor)
                {
                    // Add to start nodes.
                    startNodes.Add(i);

                    // Recursively mark this and all succursor nodes as "visited".
                    MarkSubgraphAsVisited(visited, i);
                }
            }

            // All non-visited nodes start in a cycle with no clear start node.
            // Mark the lowest-index ones of each cycle.
            for (int i = 0; i < Count; i++)
            {
                // If this node hasn't been visited yet, it is a start node.
                if (!visited[i])
                {
                    // Add to start nodes.
                    startNodes.Add(i);

                    // Recursively mark this and all succursor nodes as "visited".
                    MarkSubgraphAsVisited(visited, i);
                }
            }

            // Return found start nodes.
            return startNodes.ToArray();
        }

        /* Protected methods. */
        /// <summary>
        /// Create a new root node.
        /// </summary>
        protected virtual RootNode<DataT> CreateNode(DataT data)
        {
            return new RootNode<DataT>(data);
        }

        /* Private methods. */
        /// <summary>
        /// Mark a node and recursively mark its successor nodes.
        /// </summary>
        private void MarkSubgraphAsVisited(bool[] visited, int currentNodeIndex)
        {
            // Mark as visited.
            visited[currentNodeIndex] = true;

            // Mark successor nodes.
            RootNode<DataT> currentNode = Nodes[currentNodeIndex];
            for (int i = 0; i < currentNode.Outputs.Count; i++)
            {
                RootNode<DataT> toNode = currentNode.Outputs[i].ToNode;
                if (currentNode != null)
                {
                    int toIndex = IndexOfNode(toNode);
                    if (toIndex != -1 && !visited[toIndex])
                        MarkSubgraphAsVisited(visited, toIndex);
                }
            }
        }
    }
}