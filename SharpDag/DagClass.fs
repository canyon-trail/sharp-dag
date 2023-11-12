module SharpDag.DagClass

type Dag<'node when 'node : comparison> private (
    nodeMapIn: GraphNodeMap<'node, unit>
    ) =
    let nodeMap = nodeMapIn

    member this.Nodes = nodeMap.Nodes
    member this.Sources = nodeMap.Sources
    member this.Sinks = nodeMap.Sinks

    member this.AddNode node =
        let newNodeMap =
            nodeMap.AddNode node

        Dag(newNodeMap)

    member this.AddEdge(src, dest) =

        let srcAncestors = nodeMap.Ancestors src

        if Set.contains dest srcAncestors then
            let exc = DagCycleException "cycle found"
            raise exc
        else if src = dest then
            let exc = DagCycleException "cycle found"
            raise exc
        else

        let newNodeMap =
            nodeMap.AddEdge(src, dest)

        Dag(newNodeMap)

    member this.TopologicalSort() = seq {
        let mutable current = nodeMap
        while (current.IsEmpty |> not) do
            let currentSources = current.Sources
            yield! currentSources
            current <- current.RemoveNodes currentSources
    }

    static member FromNodes(nodesIn: 'n seq when 'n : comparison) =
        let mutable current = Dag.Empty

        for node in nodesIn do
            current <- current.AddNode node

        current

    static member Empty: Dag<'node> = Dag GraphNodeMap.Empty
