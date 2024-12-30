using System.Collections.Generic;

namespace Rusty.Graphs
{
    /// <summary>
    /// The base class for all nodes.
    /// </summary>
    public abstract class Node<DataT> where DataT : new()
    {
        /* Public properties. */
        public string Name { get; set; }
        public DataT Data { get; set; }

        /// <summary>
        /// The graph that this node is contained on.
        /// </summary>
        public abstract Graph<DataT> Graph { get; internal set; }
        /// <summary>
        /// The list of child nodes.
        /// </summary>
        public List<SubNode<DataT>> Children { get; } = new();

        /* Constructors. */
        public Node()
        {
            Data = new DataT();
        }

        public Node(DataT data)
        {
            Data = data;
        }

        public Node(DataT data, List<SubNode<DataT>> children)
        {
            Data = data;
            Children = new(children);
        }

        /* Public methods. */
        /// <summary>
        /// Remove this node from its graph, removing all of its connections in the process.
        /// </summary>
        public abstract void Remove();

        /// <summary>
        /// Removes the node from the graph, while preserving transitive connections where possible.
        /// </summary>
        public abstract void Dissolve();

        /// <summary>
        /// Add a child node. This removes the root node from the graph.
        /// </summary>
        public virtual void AddChild(RootNode<DataT> rootNode)
        {
            SubNode<DataT> child = rootNode;
            Children.Add(child);
            child.Parent = this;
            rootNode.Remove();
        }

        /// <summary>
        /// Add a child node. This removes the sub-node from its old parent.
        /// </summary>
        public void AddChild(SubNode<DataT> subNode)
        {
            subNode.Remove();
            Children.Add(subNode);
            subNode.Parent = this;
        }
    }
}