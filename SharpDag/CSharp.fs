namespace SharpDag.CSharp

type FSDag<
    'node,'edge
        when 'node: comparison
        and 'edge: comparison
    > = SharpDag.FSharp.Dag<'node,'edge>

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
    member this.AddEdge(src, dest, edge) = Dag(dag.AddEdge src dest edge)
    /// Adds the given edges the graph and returns the new graph.
    /// Ignores any edges where the same src,dest,edge tuple already
    /// exist in the graph.
    member this.AddEdges(edges) = Dag(dag.AddEdges edges)
    /// Returns nodes in a topological ordering.
    member this.TopologicalSort() = dag.TopologicalSort()
    /// Gets an empty Dag.
    static member Empty = Dag(FSDag<'node, 'edge>.Empty)
    /// Constructs a Dag from a collection of nodes.
    static member FromNodes nodes =
        Dag<'node, 'edge>.Empty.AddNodes(nodes)
    /// Constructs a Dag from a collection of edges. If any edge
    /// in the collection would cause a cycle, throws an exception.
    static member FromEdges edges =
        Dag<'node, 'edge>.Empty.AddEdges(edges)

    static member FromUntypedEdges edges =
        let typedEdges =
            edges
            |> Seq.map (fun edge -> (fst edge, snd edge, ()))
        Dag<'node, 'unit>.Empty.AddEdges(typedEdges)
