using SharpDag.CSharp;
using Unit = Microsoft.FSharp.Core.Unit;

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
        ExpectCycle(("a", "b"), ("b", "a"));
    }

    [Fact]
    public void RejectsSelfReference()
    {
        /*
            a ---
            ^    |
            |____|
         */
        ExpectCycle(("a", "a"));
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
            ("a", "b"),
            ("b", "c"),
            ("c", "a")
            );
    }

    private void ExpectCycle<T>(params (T, T)[] edges)
        where T : IComparable
    {
        Tuple<T, T> ToEdgeTuple((T, T) edge)
        {
            var (a, b) = edge;

            return Tuple.Create(a, b);
        }
        new Action(() => Dag<T, Unit>.FromUntypedEdges(edges.Select(ToEdgeTuple)))
            .Should().Throw<Exceptions.DagCycleException>();
    }


}
