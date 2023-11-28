# SharpDag

SharpDag is a library that provides a simple, straightforward implementation
of a _directed acyclic graph_ (DAG). Two key features of SharpDag are:

* Cycle Prevention - SharpDag will not accommodate an edge between nodes
  that causes a cycle
* Topological Sort - One can easily get an enumeration of nodes that are
  sorted topologically

## C# API

The namespace `SharpDag.CSharp` contains types that are conducive to use in C#.

## F# API

The namespace `SharpDag.FSharp` contains types that are conducive to use in F#.
