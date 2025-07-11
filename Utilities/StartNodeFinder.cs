using System.Collections.Generic;

namespace Rusty.Graphs;

/// <summary>
/// An utility for finding start nodes in classes that implement IGraph.
/// </summary>
public static class StartNodeFinder
{
    /* Public methods. */
    /// <summary>
    /// Find and return the indices of all start nodes in the graph. This will include all nodes without a precursor. Cycles
    /// with no clear start node will get their node with the lowest index marked as a start node.
    /// </summary>
    public static int[] FindStartNodes(IGraph graph)
    {
        // Return empty array if graph is null.
        if (graph == null)
            return [];

        // Initialize start nodes list and visited array.
        List<int> startNodes = new();
        bool[] visited = new bool[graph.NodeCount];

        // Find non-cycle start nodes.
        for (int i = 0; i < graph.NodeCount; i++)
        {
            // Get node at index.
            IRootNode node = graph.GetNodeAt(i);

            // Check if this node has at least one precursor node.
            bool hasPrecursor = false;
            for (int j = 0; j < node?.InputCount; j++)
            {
                if (node.GetInputAt(j)?.From?.Node != null)
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
                MarkSubgraphAsVisited(graph, visited, i);
            }
        }

        // All non-visited nodes start in a cycle with no clear start node.
        // Mark the lowest-index ones of each cycle.
        for (int i = 0; i < graph.NodeCount; i++)
        {
            // If this node hasn't been visited yet, it is a start node.
            if (!visited[i])
            {
                // Add to start nodes.
                startNodes.Add(i);

                // Recursively mark this and all succursor nodes as "visited".
                MarkSubgraphAsVisited(graph, visited, i);
            }
        }

        // Return found start nodes.
        return startNodes.ToArray();
    }

    /* Private methods. */
    /// <summary>
    /// Mark a node and recursively mark its successor nodes.
    /// </summary>
    private static void MarkSubgraphAsVisited(IGraph graph, bool[] visited, int currentNodeIndex)
    {
        // Mark current node as visited.
        visited[currentNodeIndex] = true;

        // Mark successor nodes.
        IRootNode currentNode = graph.GetNodeAt(currentNodeIndex);
        for (int i = 0; i < currentNode?.OutputCount; i++)
        {
            IRootNode toNode = currentNode?.GetOutputAt(i)?.To?.Node;
            if (currentNode != null)
            {
                int toIndex = graph.IndexOfNode(toNode);
                if (toIndex != -1 && !visited[toIndex])
                    MarkSubgraphAsVisited(graph, visited, toIndex);
            }
        }
    }
}