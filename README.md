# SharpDag

SharpDag is a library that provides a simple, straightforward implementation
of a [_directed acyclic graph_ (DAG)](https://en.wikipedia.org/wiki/Directed_acyclic_graph). Two key features of SharpDag are:

* Cycle Prevention - SharpDag will not allow you to add an edge between nodes
  that causes a cycle
* Topological Sort - You can easily get an `IEnumerable<T>` of all nodes such that
  no node precedes any other node with an edge that points to it.

SharpDag is written in F#, but has a C#-friendly API wrapper as well.

## C# API

The namespace `SharpDag.CSharp` contains types that work well in C#.
Consider a graph that looks like this (assume arrows point downward):

```
    A
   / \
  /   \
 B     C
  \   /
   \ /
    D
```

You can represent this graph in SharpDag like this:

```csharp
using SharpDag.CSharp;

var edges = new[] { 
    Edge.Untyped(src: "B", dest: "D"),
    Edge.Untyped(src: "C", dest: "D"),
    Edge.Untyped(src: "A", dest: "B"),
    Edge.Untyped(src: "A", dest: "C"),
};

var dag = Dag.FromUntypedEdges(edges);

var ordering = dag.TopologicalSort();
// returns "A", "B", "C", "D"
```
SharpDag does not allow cycles (that whole "acyclic" part of _directed acyclic graph_ 😉).
If you attempt to create a graph with a cycle, a `DagCycleException` will be thrown:


```csharp
using SharpDag.CSharp;

// throws an exception since A -> B -> C -> A is a cycle
var dag = Dag.FromUntypedEdges(new[] {
    Edge.Untyped(src: "A", dest: "B"), 
    Edge.Untyped(src: "B", dest: "C"), 
    Edge.Untyped(src: "C", dest: "A"), 
});
```

## F# API

The namespace `SharpDag.FSharp` contains types that work well in F#.
Consider a graph that looks like this (assume arrows point downward):

```
    A
   / \
  /   \
 B     C
  \   /
   \ /
    D
```
F# code to represent this graph would look like this:
```csharp
open SharpDag.FSharp

let dag =
    Dag.empty
    |> Dag.addEdge "B" "D" ()
    |> Dag.addEdge "C" "D" ()
    |> Dag.addEdge "A" "B" ()
    |> Dag.addEdge "A" "C" ()

let ordering = Dag.topologicalSort dag
// returns "A", "B", "C", "D"
```
## "Typed" versus "Untyped"

All edges in SharpDag have a data value associated with them. Since the implementation
language is F#, we can simply use the `unit` type to represent when we don't care about
storing a data value with the edge (like the examples above).

Since C# doesn't have a similar concept to F#'s `unit` type, the C# API has various
methods referring to either "typed" or "untyped" edges. The "untyped" APIs are
a convenience to simplify creating graphs and edges that use F#'s `unit` type under the hood.
