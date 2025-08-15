namespace Rusty.Graphs;

/// <summary>
/// A node that can be contained within another node. Has no inputs/outputs, but can have sub-nodes of its own.
/// </summary>
public class SubNode : Node, ISubNode
{
    /* Public properties. */
    public INode Parent { get; set; }
    public IRootNode Root
    {
        get
        {
            if (Parent is IRootNode root)
                return root;
            else if (Parent is ISubNode parent)
                return parent.Root;
            else
                return null;
        }
    }
    public override IGraph Graph
    {
        get => Root.Graph;
        set
        {
            if (Root != null)
                Root.Graph = value;
        }
    }

    /* Public methods. */
    public override string ToString()
    {
        return Serializer.ToString(this);
    }


    public override void Remove()
    {
        if (Parent != null && Parent.ContainsChild(this))
            Parent.RemoveChild(this);
        Parent = null;
    }

    public override void Dissolve()
    {
        if (ChildCount > 0)
        {
            ISubNode replace = GetChildAt(0);
            RemoveChild(replace);
            Parent.ReplaceChild(this, replace);
        }
        else
            Remove();
    }
}