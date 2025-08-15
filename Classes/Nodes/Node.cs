using System.Collections.Generic;

namespace Rusty.Graphs;

/// <summary>
/// The base class for all nodes.
/// </summary>
public abstract class Node : INode
{
    /* Public properties. */
    public INodeData Data { get; set; }
    public abstract IGraph Graph { get; set; }

    public int ChildCount => Children.Count;

    /* Private properties. */
    private List<ISubNode> Children { get; } = new();

    /* Public methods. */
    public bool ContainsChild(ISubNode child)
    {
        return Children.Contains(child);
    }

    public int GetChildIndex(ISubNode child)
    {
        return Children.IndexOf(child);
    }

    public virtual ISubNode GetChildAt(int index)
    {
        return Children[index];
    }

    public virtual ISubNode CreateChild()
    {
        SubNode child = new();
        AddChild(child);
        return child;
    }

    public void AddChild(ISubNode node)
    {
        node.Remove();
        Children.Add(node);
        node.Parent = this;
    }

    public virtual ISubNode AddChild(IRootNode node)
    {
        node.Remove();

        ISubNode child = node.ToChild();
        AddChild(child);
        child.StealChildren(node);
        return child;
    }

    public void InsertChild(int index, ISubNode node)
    {
        AddChild(node);
        MoveChild(node, index);
    }

    public virtual ISubNode InsertChild(int index, IRootNode node)
    {
        ISubNode child = AddChild(node);
        MoveChild(child, index);
        return child;
    }

    public void ReplaceChild(ISubNode oldChild, ISubNode newChild)
    {
        InsertChild(GetChildIndex(oldChild), newChild);
        RemoveChild(oldChild);
    }

    public virtual ISubNode ReplaceChild(ISubNode oldChild, IRootNode newChild)
    {
        int index = GetChildIndex(oldChild);
        ISubNode child = InsertChild(index, newChild);
        RemoveChild(oldChild);
        return child;
    }

    public void MoveChild(ISubNode child, int toIndex)
    {
        if (!ContainsChild(child))
            return;

        int fromIndex = GetChildIndex(child);
        Children.RemoveAt(fromIndex);

        if (fromIndex < toIndex)
            toIndex--;

        Children.Insert(toIndex, child);
    }

    public void RemoveChild(ISubNode child)
    {
        Children.Remove(child);
    }

    public void ClearChildren()
    {
        while (Children.Count > 0)
        {
            RemoveChild(GetChildAt(0));
        }
    }

    public void StealChildren(INode from)
    {
        while (from.ChildCount > 0)
        {
            AddChild(from.GetChildAt(0));
        }
    }

    public abstract void Remove();

    public abstract void Dissolve();
}