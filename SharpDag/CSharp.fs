namespace SharpDag.CSharp

type FSDag<
    'node,'edge
        when 'node: comparison
        and 'edge: comparison
    > = SharpDag.FSharp.Dag<'node,'edge>

/// Represents an edge with a source node, destination node, and edge value
type TypedEdge<
    'node,'edge
        when 'node: comparison
        and 'edge: comparison
    >(src: 'node, dest: 'node, value: 'edge) =
    member this.Source = src
    member this.Dest = dest
    member this.Value = value

    member this.AsTuple() = (src, dest, value)

/// Represents an edge with a source node, destination node, but no edge value
type UntypedEdge<'node when 'node : comparison>(src: 'node, dest: 'node) =
    member this.Source = src
    member this.Dest = dest

    member this.AsTyped() = TypedEdge(src, dest, ())
    member this.AsTuple() = (src, dest, ())
type Edge =
    static member Untyped<'node when 'node : comparison>(src: 'node, dest: 'node) =
        UntypedEdge(src, dest)

    static member Typed<
        'node, 'edge
            when 'node : comparison
            and 'edge : comparison
        >(src: 'node, dest: 'node, edge: 'edge) =
        TypedEdge(src, dest, edge)

type Dag<
    'node, 'edge
        when 'node : comparison
        and 'edge : comparison
    >
    private (dagIn: FSDag<'node, 'edge>) =
    let dag = dagIn

    /// Gets the set of all nodes in the graph.
    member this.Nodes =
        dag.Nodes
        :> 'node seq

    /// Gets the set of all nodes in the graph that have in-degree of zero;
    /// that is, no incoming edges.
    member this.Sources =
        dag.Sources
        :> 'node seq
    /// Gets the set of all nodes in the graph that have out-degree of zero;
    /// that is, no outgoing edges.
    member this.Sinks =
        dag.Sinks
        :> 'node seq
    /// Creates a new graph that includes the given node. If it already exists
    /// in the graph, this function is effectively a no-op.
    member this.AddNode(node) = Dag(dag.AddNode node)
    /// Creates a new graph that includes the given nodes. Ignores
    /// nodes that already exist in the graph.
    member this.AddNodes(nodes) = Dag(dag.AddNodes nodes)
    /// Adds the given edge from src to dest to the graph and returns it.
    /// If an edge already exists with the same src, dest, and edge values,
    /// returns the original graph. If the edge would create a cycle,
    /// throws an exception.
    member this.AddEdge(edge: TypedEdge<'node, 'edge>) = Dag(dag.AddEdge edge.Source edge.Dest edge.Value)
    /// Adds the given edges the graph and returns the new graph.
    /// Ignores any edges where the same src,dest,edge tuple already
    /// exist in the graph.
    member this.AddEdges(edges: TypedEdge<'node, 'edge> seq) =
        let tupledEdges =
            edges
            |> Seq.map (fun e -> e.AsTuple())
        Dag(dag.AddEdges tupledEdges)
    /// Returns nodes in a topological ordering.
    member this.TopologicalSort() = dag.TopologicalSort()
    /// Returns a new Dag with the given nodes removed
    member this.RemoveNodes(nodes) = dag.RemoveNodes nodes
    /// Returns a new Dag with the given node removed
    member this.RemoveNode(node) =
        let nodes = Seq.singleton node
        dag.RemoveNodes nodes
    /// Gets an empty Dag.
    static member Empty = Dag(FSDag<'node, 'edge>.Empty)
    /// Constructs a Dag from a collection of nodes.
    static member FromNodes nodes =
        Dag<'node, 'edge>.Empty.AddNodes(nodes)
    /// Constructs a Dag from a collection of edges. If any edge
    /// in the collection would cause a cycle, throws an exception.
    static member FromEdges edges =
        Dag<'node, 'edge>.Empty.AddEdges(edges)

type Dag<'node when 'node : comparison> =
    /// Gets an empty Dag.
    static member Empty = Dag<'node, unit>.Empty
    /// Constructs a Dag from a collection of nodes.
    static member FromNodes nodes =
        Dag<'node, unit>.Empty.AddNodes(nodes)
    /// Constructs a Dag from a collection of edges. If any edge
    /// in the collection would cause a cycle, throws an exception.
    static member FromEdges (edges: UntypedEdge<'node> seq) =
        let typedEdges =
            edges
            |> Seq.map (fun edge -> edge.AsTyped())
        Dag<'node, unit>.FromEdges(typedEdges)

type Dag =
    /// Constructs a Dag from a collection of nodes.
    static member FromNodes<'node when 'node : comparison> nodes =
        Dag<'node, unit>.Empty.AddNodes(nodes)

    /// Constructs a Dag from a collection of untyped edges.
    static member FromUntypedEdges<
        'node when 'node : comparison
    > (edges: UntypedEdge<'node> seq) =
        Dag<'node>.FromEdges(edges)

    /// Constructs a Dag from a collection of typed edges.
    static member FromTypedEdges<
        'node, 'edge
            when 'node : comparison
            and 'edge : comparison
        > (edges: TypedEdge<'node, 'edge> seq) =
        Dag<'node, 'edge>.FromEdges(edges)
