namespace SharpDag

/// Represents a node in a graph. Maintains maps of
/// inbound and outbound edges.
type GraphNode<
    'node, 'edge
        when 'node : comparison
        and 'edge : comparison
    > private (
        keyIn,
        inEdgesIn: NodeEdgeMap<'node, 'edge>,
        outEdgesIn: NodeEdgeMap<'node, 'edge>
    ) =
    let key = keyIn
    let inEdges = inEdgesIn
    let outEdges = outEdgesIn

    member this.Key: 'node = key

    member this.AddInEdge (src, edge) =
        let newInEdges = inEdges.AddEdge(src, edge)

        GraphNode(key, newInEdges, outEdges)

    member this.AddOutEdge (dest, edge) =
        let newOutEdges = outEdges.AddEdge(dest, edge)

        GraphNode(key, inEdges, newOutEdges)

    member this.RemoveReferencesTo node =
        let newInEdges =
            inEdges.RemoveNode node
        let newOutEdges =
            outEdges.RemoveNode node

        GraphNode(this.Key, newInEdges, newOutEdges)

    member this.ConnectedNodes =
        [|
            inEdges.ConnectedNodes;
            outEdges.ConnectedNodes;
        |]
        |> Seq.collect id
        |> Set.ofSeq

    member this.ParentNodes =
        inEdges.AdjacentNodes

    member this.ChildNodes =
        outEdges.AdjacentNodes
    member this.IsSource = inEdges.IsEmpty
    member this.IsSink = outEdges.IsEmpty

    static member ForNodeValue node = GraphNode(node, NodeEdgeMap.Empty, NodeEdgeMap.Empty)

module GraphNode =
    let addInEdge src edge (node: GraphNode<_,_>) =
        node.AddInEdge(src, edge)

    let addOutEdge dest edge (node: GraphNode<_,_>) =
        node.AddOutEdge(dest, edge)

    let isSource (node: GraphNode<'n, 'e>) = node.IsSource
    let isSink (node: GraphNode<'n, 'e>) = node.IsSink
