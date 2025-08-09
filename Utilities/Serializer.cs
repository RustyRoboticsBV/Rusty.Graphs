using System.Collections.Generic;
using System;

namespace Rusty.Graphs;

/// <summary>
/// An utility for converting objects that implement IGraph or INode to a text format for debugging purposes.
/// </summary>
public static class Serializer
{
    /* Public methods. */
    /// <summary>
    /// Convert an IGraph to string.
    /// </summary>
    public static string ToString(IGraph graph)
    {
        string str = "";
        HashSet<INode> examined = new();
        for (int i = 0; i < graph.NodeCount; i++)
        {
            if (!examined.Contains(graph.GetNodeAt(i)))
            {
                if (str != "")
                    str += "\n";
                str += ToString(graph.GetNodeAt(i), examined, true);
            }
        }
        return str;
    }

    /// <summary>
    /// Convert an IRootNode to string.
    /// </summary>
    public static string ToString(IRootNode node)
    {
        return ToString(node, new(), true);
    }

    /// <summary>
    /// Convert an ISubNode to string.
    /// </summary>
    public static string ToString(ISubNode node)
    {
        return ToString(node, new());
    }

    /* Private methods. */
    /// <summary>
    /// Stringify a IRootNode.
    /// </summary>
    private static string ToString(IRootNode node, HashSet<INode> examined, bool includeOutputs)
    {
        string str = "";
        int width = 0;

        // Detect cycles.
        bool examinedAlready = examined.Contains(node);
        if (examinedAlready)
        {
            string data = node.Data != null ? node.Data.ToString() : "";
            str = $"({data}...)";
            width = str.Length;
        }

        else
        {
            // Base stringify.
            str = ToString(node, examined);

            // Make every line the same width.
            width = GetWidth(str);
            string[] substrs = str.Split('\n');
            str = "";
            for (int i = 0; i < substrs.Length; i++)
            {
                substrs[i] += new string(' ', width - substrs[i].Length);
                if (i > 0)
                    str += '\n';
                str += substrs[i];
            }
        }

        // Enclose in borders.
        str = '│' + str.Replace("\n", "│\n│") + '│';
        str = '┌' + new string('─', width) + '┐'
            + $"\n{str}\n"
            + '└' + new string('─', width) + '┘';

        // Stop here if we already examined this node.
        if (examinedAlready)
            return str;

        // Add title line.
        if (node.ChildCount > 0)
            str = str.Insert(width * 2 + 6, '├' + new string('─', width) + '┤' + '\n');

        // Handle outputs.
        if (includeOutputs)
        {
            // Add bottom connector.
            if (node.OutputCount > 0)
                str = str.Replace("└─", "└┬");

            // Alter output of output node to connect and indent it.
            for (int i = 0; i < node.OutputCount; i++)
            {
                IInputPort to = node.GetOutputAt(i).To;
                string childStr = "";
                if (to == null || to.Node == null)
                    childStr = "(null)";
                else
                    childStr = ToString(node.GetOutputAt(i).To.Node, examined, true);

                childStr = " │" + childStr.Replace("\n", "\n  ");
                if (i < node.OutputCount - 1)
                {
                    childStr = ReplaceFirst(childStr, "\n  │", "\n ├┤");
                    childStr = childStr.Replace("\n  ", "\n │");
                }
                else
                    childStr = ReplaceFirst(childStr, "\n  │", "\n └┤");

                str += '\n' + childStr;
            }
        }

        return str;
    }

    /// <summary>
    /// Stringify a generic INode.
    /// </summary>
    private static string ToString(INode node, HashSet<INode> examined)
    {
        string data = node.Data != null ? node.Data.ToString() : "";

        // Detect cycles.
        if (examined.Contains(node))
            return $"({data}...)";
        examined.Add(node);

        // Stringify this node.
        string str = data;

        // Stringify children.
        for (int i = 0; i < node.ChildCount; i++)
        {
            string childStr = ToString(node.GetChildAt(i), examined);

            if (i == node.ChildCount - 1)
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
    /// Get the width of a string (which potentially contains line-breaks).
    /// </summary>
    private static int GetWidth(string str)
    {
        if (!str.Contains('\n'))
            return str.Length;

        string[] substrs = str.Split('\n');
        int width = 0;
        foreach (string substr in substrs)
        {
            if (substr.Length > width)
                width = substr.Length;
        }
        return width;
    }

    /// <summary>
    /// Replace the first occurance of a substring within a string.
    /// </summary>
    private static string ReplaceFirst(string str, string oldValue, string newValue)
    {
        int pos = str.IndexOf(oldValue);
        if (pos < 0)
            return str;
        return str.Substring(0, pos) + newValue + str.Substring(pos + oldValue.Length);
    }
}