using System.Collections.Generic;

namespace Rusty.Graphs
{
    /// <summary>
    /// The base class for all nodes.
    /// </summary>
    public abstract class Node<DataT> where DataT : new()
    {
        /* Public properties. */
        public DataT Data { get; set; }
        public List<SubNode<DataT>> Children { get; private set; } = new();
        public List<(string, object)> Attributes { get; } = new();

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
        /// Remove this node from the graph, removing all of its connections.
        /// </summary>
        public abstract void Remove();

        /// <summary>
        /// Dissolve this node (if possible). This preserves connections between other nodes.
        /// </summary>
        public abstract void Dissolve();

        public virtual void AddChild(RootNode<DataT> rootNode)
        {
            SubNode<DataT> child = rootNode;
            Children.Add(child);
            child.Parent = this;
        }

        public void AddChild(SubNode<DataT> subNode)
        {
            Children.Add(subNode);
            subNode.Parent = this;
        }
    }
}