[<AutoOpen>]
module SharpDag.Dag

open DagClass

let fromEdges (edges: ('n * 'n) seq when 'n : comparison) =
    let mutable current = Dag<'n>.Empty

    for edge in edges do
        current <- current.AddEdge(edge)

    current

let fromNodes (nodes: 'n seq when 'n : comparison) =
    let mutable current = Dag<'n>.Empty

    for node in nodes do
        current <- current.AddNode node

    current
