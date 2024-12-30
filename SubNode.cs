using System.Collections.Generic;

namespace Rusty.Graphs
{
    /// <summary>
    /// A node that can be contained within another node. Has no inputs/outputs, but can have sub-nodes of its own.
    /// </summary>
    public class SubNode<DataT> : Node<DataT>
        where DataT : new()
    {
        /* Public properties. */
        public Node<DataT> Parent { get; internal set; }

        /* Constructors. */
        public SubNode() : base() { }

        public SubNode(DataT data) : base(data) { }

        public SubNode(DataT data, List<SubNode<DataT>> children) : base(data, children) { }

        /* Public methods. */
        public override void Remove()
        {
            if (Parent.Children.Contains(this))
                Parent.Children.Remove(this);
        }

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