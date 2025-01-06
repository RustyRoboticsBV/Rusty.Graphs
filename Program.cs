using Rusty.Graphs;
using System;

var graph = new Graph<int>();

var node1 = graph.AddNode("KAAS");

var node2 = graph.AddNode("EI\n2");
node1.Consume(node2);

var node3 = graph.AddNode("BOTER");
node1.Consume(node3);

var node4 = graph.AddNode("PIJN   ");
node1.Children[0].Consume(node4);

graph.AddNode("BALLS");
node1.ConnectTo(graph.Nodes[^1]);
node1.ConnectTo(graph.Nodes[^1]);

var bossNode = graph.AddNode("BOSS");
bossNode.Consume(node1);
Console.WriteLine(graph);