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

    public ISubNode GetChildAt(int index)
    {
        return Children[index];
    }

    public virtual ISubNode CreateChild()
    {
        SubNode child = new();
        AddChild(child);
        return child;
    }

    public void AddChild(INode node)
    {
        ISubNode child = ConvertToChild(node);
        Children.Add(child);
        child.Parent = this;
    }

    public void InsertChild(int index, INode node)
    {
        ISubNode child = ConvertToChild(node);
        Children.Insert(index, child);
        child.Parent = this;
    }

    public void ReplaceChild(ISubNode oldChild, INode newChild)
    {
        InsertChild(GetChildIndex(oldChild), newChild);
        RemoveChild(oldChild);
    }

    public void MoveChild(ISubNode child, int toIndex)
    {
        if (!ContainsChild(child))
            return;

        int fromIndex = GetChildIndex(child);
        RemoveChild(child);
        if (toIndex > fromIndex)
            toIndex--;
        InsertChild(toIndex, child);
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

    /* Private methods. */
    /// <summary>
    /// If a root-node, delete it and transfer its children to a new sub-node.
    /// Else, remove it from its parent and return the node as-is.
    /// </summary>
    private static ISubNode ConvertToChild(INode node)
    {
        SubNode child = new();
        child.Data = node.Data.Copy();
        child.StealChildren(node);
        node.Remove();
        return child;
    }
}