namespace SharpDag

type GraphNodeMap<
    'node, 'edge
        when 'node : comparison
        and 'edge : comparison
    > private (mapIn) =
    let map = mapIn

    let getNode node =
        map
        |> Map.tryFind node
        |> Option.defaultValue (GraphNode.ForNodeValue node)

    let addNode node =
        let n = getNode node

        let newMap =
            map
            |> Map.add node n

        GraphNodeMap(newMap)

    let addEdge src dest =
        let srcNode =
            getNode src
            |> GraphNode.addOutEdge dest ()
        let destNode =
            getNode dest
            |> GraphNode.addInEdge src ()

        let newMap =
            map
            |> Map.add src srcNode
            |> Map.add dest destNode

        GraphNodeMap(newMap)

    let removeNode node =
        let nodeVal = getNode node

        let referrers =
            nodeVal.ConnectedNodes
            |> Seq.map (fun x -> Map.find x map)
            |> Seq.map (fun x -> x.RemoveReferencesTo(node))

        let mutable newMap =
            map
            |> Map.remove node

        for referrer in referrers do
            newMap <- Map.add referrer.Key referrer newMap

        GraphNodeMap(newMap)

    let rec ancestors node =
        let nodeVal = getNode node
        let parents = nodeVal.ParentNodes

        let extendedAncestors =
            parents
            |> Seq.map ancestors
            |> Seq.fold Set.union Set.empty

        parents
        |> Set.union extendedAncestors

    let rec descendants node =
        let nodeVal = getNode node
        let parents = nodeVal.ChildNodes

        let extendedDescendants =
            parents
            |> Seq.map descendants
            |> Seq.fold Set.union Set.empty

        parents
        |> Set.union extendedDescendants


    member this.Nodes =
        Map.keys map
        |> Set.ofSeq

    member this.Sources =
        map
        |> Map.values
        |> Seq.filter GraphNode.isSource
        |> Seq.map (fun n -> n.Key)
        |> Set.ofSeq

    member this.Sinks =
        map
        |> Map.values
        |> Seq.filter GraphNode.isSink
        |> Seq.map (fun n -> n.Key)
        |> Set.ofSeq

    member this.GetNode node = getNode node
    member this.AddNode node = addNode node
    member this.AddEdge(src, dest) = addEdge src dest
    member this.RemoveNode node = removeNode node
    member this.RemoveNodes nodes =
        let mutable result = this

        for n in nodes do
            result <- result.RemoveNode n

        result

    member this.Ancestors node = ancestors node
    member this.Descendants node = descendants node
    member this.IsEmpty = Map.isEmpty map
    static member Empty = GraphNodeMap Map.empty
