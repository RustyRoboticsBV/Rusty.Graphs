using System.Collections.Generic;

namespace Rusty.Graphs
{
    /// <summary>
    /// A node that can be contained within another node. Has no inputs/outputs, but can have sub-nodes of its own.
    /// </summary>
    public class SubNode<DataT> : Node<DataT>
        where DataT : NodeData, new()
    {
        /* Public properties. */
        public Node<DataT> Parent { get; internal set; }
        public RootNode<DataT> Root
        {
            get
            {
                if (Parent is RootNode<DataT> root)
                    return root;
                else if (Parent is SubNode<DataT> subNode)
                    return subNode.Root;
                else
                    return null;
            }
        }
        public override Graph<DataT> Graph
        {
            get
            {
                try
                {
                    return Root.Graph;
                }
                catch
                {
                    return null;
                }
            }
            internal set
            {
                try
                {
                    Root.Graph = value;
                }
                catch { }
            }
        }

        /* Constructors. */
        public SubNode() : base() { }

        public SubNode(DataT data) : base(data) { }

        public SubNode(DataT data, List<SubNode<DataT>> children) : base(data, children) { }

        /* Public methods. */
        /// <summary>
        /// Removes the sub-node and all of its children from the graph.
        /// </summary>
        public override void Remove()
        {
            if (Parent != null && Parent.Children.Contains(this))
                Parent.Children.Remove(this);
        }

        /// <summary>
        /// Removes the sub-node from the graph. Its first child is added as a child of the parent node.
        /// </summary>
        public override void Dissolve()
        {
            if (Children.Count > 0)
            {
                int index = Parent.Children.IndexOf(this);
                if (index != -1)
                    Parent.Children[index] = Children[0];
            }
            Remove();
        }
    }
}