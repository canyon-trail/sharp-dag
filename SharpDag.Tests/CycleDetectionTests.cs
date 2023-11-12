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
        new Action(() => Dag.fromEdges(edges.Select(x => x.ToTuple())))
            .Should().Throw<Exceptions.DagCycleException>();
    }


}
