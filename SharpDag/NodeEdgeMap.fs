namespace SharpDag

/// Represents a map of edges for a specific node in a graph.
/// Nodes will have two NodeEdgeMaps, one for inbound and one
/// for outbound edges. An edge is the combination of the connected
/// node and the edge value. Adding the same node * edge combination
/// is effectively a no-op.
type NodeEdgeMap<
    'node, 'edge
        when 'node : comparison
        and 'edge : comparison
    > private (edgeMapIn) =
    let edgeMap: Map<'node, Set<'edge>> = edgeMapIn

    member this.AddEdge (node, edge) =
        let edgeValues =
            edgeMap
            |> Map.tryFind node
            |> Option.defaultValue Set.empty
            |> Set.add edge

        let newEdgeMap =
            edgeMap
            |> Map.add node edgeValues

        NodeEdgeMap(newEdgeMap)

    member this.RemoveNode node =
        let newEdgeMap =
            edgeMap
            |> Map.remove node

        NodeEdgeMap(newEdgeMap)

    member this.ConnectedNodes =
        edgeMap
        |> Map.keys

    member this.AdjacentNodes =
        edgeMap.Keys
        |> Set.ofSeq
    member this.IsEmpty = Map.isEmpty edgeMap

    static member Empty = NodeEdgeMap(Map.empty)
