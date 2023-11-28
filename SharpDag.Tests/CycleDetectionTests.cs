using SharpDag.CSharp;

namespace SharpDag.Tests;

public sealed class CycleDetectionTests
{

    [Fact]
    public void RejectsSimpleCycle()
    {
        /*
            a -> b
            ^    |
            |____|
         */
        ExpectCycle(
            Edge.Untyped("a", "b"),
            Edge.Untyped("b", "a")
            );
    }

    [Fact]
    public void RejectsSelfReference()
    {
        /*
            a ---
            ^    |
            |____|
         */
        ExpectCycle(Edge.Untyped("a", "a"));
    }

    [Fact]
    public void RejectsCycleWithTwoHops()
    {
        /*
            a -> b -> c
            ^         |
            |_________|
         */
        ExpectCycle(
            Edge.Untyped("a", "b"),
            Edge.Untyped("b", "c"),
            Edge.Untyped("c", "a")
            );
    }

    private void ExpectCycle<T>(params UntypedEdge<T>[] edges)
        where T : IComparable
    {
        new Action(() => Dag.FromUntypedEdges(edges))
            .Should().Throw<Exceptions.DagCycleException>();
    }


}
