# Rusty.Graphs
A generic, directed, hierarchical graph module, written in C#.

It contains several interfaces and base classes for several graph-related concepts.

## Concepts
We use the following concepts:
- Graphs: a collection of nodes.
- Nodes: elements on the graph that contain data and connections to other nodes.
  - Root nodes: nodes that are direct children of a graph. They can have inputs, outputs, children and data.
  - Child nodes: nodes that are a child of another node. They can have data and children of their own, but don't have inputs or outputs.
- Node data: a block of data that can be contained within a node.
- Ports: connection sockets between root nodes.
  - Input ports: an end point of a connection.
  - Output ports: a start point of a connection.
