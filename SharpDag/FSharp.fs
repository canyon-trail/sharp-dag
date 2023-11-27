namespace SharpDag.FSharp

open SharpDag

type Dag<
    'node, 'edge
        when 'node : comparison
        and 'edge : comparison
    > (mapIn: GraphNodeMap<'node, 'edge>) =
    let map = mapIn
    static member Empty: Dag<'node, 'edge> = Dag(GraphNodeMap.Empty)
    member this.Nodes = map.Nodes
    member this.Sources = map.Sources
    member this.Sinks = map.Sinks
    member this.AddNode node =
        let newNodeMap =
            map.AddNode node

        Dag(newNodeMap)
    member this.AddEdge src dest edge =
        let srcAncestors = map.Ancestors src

        if Set.contains dest srcAncestors then
            let exc = DagCycleException "cycle found"
            raise exc
        else if src = dest then
            let exc = DagCycleException "cycle found"
            raise exc
        else

        let newNodeMap =
            map.AddEdge(src, dest, edge)

        Dag(newNodeMap)

    member this.AddNodes nodes =
        let mutable result = this

        for node in nodes do
            result <- result.AddNode node

        result
    member this.AddEdges edges =
        let mutable result = this

        for src, dest, edge in edges do
            result <- result.AddEdge src dest edge

        result

    member this.TopologicalSort() = seq {
        let mutable current = map
        while (current.IsEmpty |> not) do
            let currentSources = current.Sources
            yield! currentSources
            current <- current.RemoveNodes currentSources
    }
    member this.IsEmpty = map.IsEmpty

    member this.RemoveNodes nodes = Dag(map.RemoveNodes nodes)

module Dag =
    /// Gets the set of all nodes in the graph.
    let nodes (dag: Dag<_,_>) = dag.Nodes
    /// Gets the set of all nodes in the graph that have in-degree of zero;
    /// that is, no incoming edges.
    let sources (dag: Dag<_,_>) = dag.Sources
    /// Gets the set of all nodes in the graph that have out-degree of zero;
    /// that is, no outgoing edges.
    let sinks (dag: Dag<_,_>) = dag.Sinks
    /// Creates a new graph that includes the given node. If it already exists
    /// in the graph, this function returns the same graph.
    let addNode node (dag: Dag<_,_>) = dag.AddNode node
    /// Adds the given edge from src to dest to the graph and returns it.
    /// If an edge already exists with the same src, dest, and edge values,
    /// returns the original graph. If the edge would create a cycle,
    /// throws an exception.
    let addEdge src dest edge (dag: Dag<_,_>) = dag.AddEdge src dest edge
    /// Returns nodes in a topological ordering.
    let topologicalSort (dag: Dag<_,_>) = dag.TopologicalSort()
    /// Gets an empty Dag.
    let empty<'node,'edge when 'node : comparison and 'edge : comparison> = Dag<'node, 'edge>.Empty
    /// Constructs a Dag from a collection of nodes.
    let fromNodes nodes =
        empty.AddNodes nodes
    /// Constructs a Dag from a collection of edges. If any edge
    /// in the collection would cause a cycle, throws an exception.
    let fromEdges edges =
        empty.AddEdges edges
