using SharpDag.CSharp;
using Unit = Microsoft.FSharp.Core.Unit;

namespace SharpDag.Tests;
using System.Linq;

public sealed class TopologicalSortTests
{
    [Fact]
    public void BaseCase()
    {
        var testee = Dag<string, Unit>.FromNodes(new[] { "a" });

        testee.TopologicalSort().Should().BeEquivalentTo(new [] { "a" });
    }

    [Fact]
    public void SingleEdge()
    {
        var testee = Dag<string, Unit>.FromUntypedEdges(new[] {("a", "b").ToTuple()});

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
            ("a", "d"),
            ("b", "d"),
            ("c", "d"),
        }.Select(x => x.ToTuple());

        var testee = Dag<string, Unit>.FromUntypedEdges(edges);

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
            ("a", "b"),
            ("a", "c"),
            ("a", "d"),
        }.Select(x => x.ToTuple());

        var testee = Dag<string, Unit>.FromUntypedEdges(edges);

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
            ("a", "b"),
            ("a", "c"),
            ("a", "d"),
            ("b", "c"),
            ("d", "e"),
        }.Select(x => x.ToTuple());

        var testee = Dag<string, Unit>.FromUntypedEdges(edges);

        var result = testee.TopologicalSort().ToList();

        var firstTier = result.Take(1);
        firstTier.Should().BeEquivalentTo(new [] { "a" });

        var secondTier = result.Skip(1).Take(2);
        secondTier.Should().BeEquivalentTo(new[] { "b", "d" });

        var thirdTier = result.Skip(3).Take(2);
        thirdTier.Should().BeEquivalentTo(new[] { "c", "e" });
    }
}
