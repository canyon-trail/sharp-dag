using SharpDag.CSharp;

namespace SharpDag.Tests;
using System.Linq;

public sealed class TopologicalSortTests
{
    [Fact]
    public void BaseCase()
    {
        var testee = Dag.FromNodes(new[] { "a" });

        testee.TopologicalSort().Should().BeEquivalentTo(new [] { "a" });
    }

    [Fact]
    public void SingleEdge()
    {
        var testee = Dag.FromUntypedEdges(new[] { Edge.Untyped("a", "b") });

        testee.TopologicalSort().Should().BeEquivalentTo(
            new[] { "a", "b" },
            x => x.WithStrictOrdering()
        );
    }

    [Fact]
    public void MultipleSources()
    {
        /*

         a
           \
            ->
         b --> d
            ->
           /
         c
         */

        var edges = new[]
        {
            Edge.Untyped("a", "d"),
            Edge.Untyped("b", "d"),
            Edge.Untyped("c", "d"),
        };

        var testee = Dag.FromUntypedEdges(edges);

        var result = testee.TopologicalSort().ToList();
        result.Take(3).Should().BeEquivalentTo(new[] { "a", "b", "c" });
        result.Last().Should().Be("d");
    }

    [Fact]
    public void MultipleSinks()
    {
        /*
             ---> b
            /
           /
         a -----> c
           \
            \
             ---> d

         */

        var edges = new[]
        {
            Edge.Untyped("a", "b"),
            Edge.Untyped("a", "c"),
            Edge.Untyped("a", "d"),
        };

        var testee = Dag.FromUntypedEdges(edges);

        var result = testee.TopologicalSort().ToList();
        result.Skip(1).Should().BeEquivalentTo(new[] { "b", "c", "d" });
        result.First().Should().Be("a");
    }

    [Fact]
    public void ThreeLevelGraph()
    {
        /*
             ---> b
            /      \
           /        --->
         a ------------> c
           \
            \
             ---> d ---> e

         */

        var edges = new[]
        {
            Edge.Untyped("a", "b"),
            Edge.Untyped("a", "c"),
            Edge.Untyped("a", "d"),
            Edge.Untyped("b", "c"),
            Edge.Untyped("d", "e"),
        };

        var testee = Dag.FromUntypedEdges(edges);

        var result = testee.TopologicalSort().ToList();

        var firstTier = result.Take(1);
        firstTier.Should().BeEquivalentTo(new [] { "a" });

        var secondTier = result.Skip(1).Take(2);
        secondTier.Should().BeEquivalentTo(new[] { "b", "d" });

        var thirdTier = result.Skip(3).Take(2);
        thirdTier.Should().BeEquivalentTo(new[] { "c", "e" });
    }
}
