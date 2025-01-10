using System.Collections.Generic;

namespace Rusty.Graphs
{
    /// <summary>
    /// The base class for all nodes.
    /// </summary>
    public abstract class Node<DataT> where DataT : NodeData, new()
    {
        /* Public properties. */
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
        public Node() : this(new DataT()) { }

        public Node(DataT data)
        {
            Data = data;
        }

        public Node(DataT data, List<SubNode<DataT>> children) : this(data)
        {
            Children = new List<SubNode<DataT>>(children);
        }

        /* Public methods. */
        public override string ToString()
        {
            return ToString(new HashSet<Node<DataT>>());
        }

        public virtual string ToString(HashSet<Node<DataT>> examined)
        {
            // Detect cycles.
            if (examined.Contains(this))
                return $"({GetName()}...)";
            examined.Add(this);

            // Stringify this node.
            string str = GetName();

            // Stringify children.
            for (int i = 0; i < Children.Count; i++)
            {
                string childStr = Children[i].ToString(examined);

                if (i == Children.Count - 1)
                {
                    childStr = '└' + childStr;
                    childStr = childStr.Replace("\n", "\n ");
                }
                else
                {
                    childStr = '├' + childStr;
                    childStr = childStr.Replace("\n", "\n│");
                }

                str += '\n' + childStr;
            }

            return str;
        }

        /// <summary>
        /// Get the name of the node. By default, this converts the data to a string and returns the result.
        /// </summary>
        public virtual string GetName()
        {
            return Data.ToString();
        }

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
        public virtual void Consume(RootNode<DataT> rootNode)
        {
            if (rootNode == null)
                return;

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
            if (Children.Contains(subNode))
                return;
            subNode.Remove();
            Children.Add(subNode);
            subNode.Parent = this;
        }
    }
}