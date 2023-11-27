using SharpDag.CSharp;

namespace SharpDag.Tests;
using Unit = Microsoft.FSharp.Core.Unit;

public sealed class SourcesSinksTests
{
    [Fact]
    public void TreatsIsolatedNodesAsSources()
    {
        var testee = Dag<string, Unit>.FromNodes(new[] { "a", "b", "c" });

        testee.Sources.Should().BeEquivalentTo(new[] { "a", "b", "c" });
    }

    [Fact]
    public void TreatsIsolatedNodesAsSinks()
    {
        var testee = Dag<string, Unit>.FromNodes(new[] { "a", "b", "c" });

        testee.Sinks.Should().BeEquivalentTo(new[] { "a", "b", "c" });
    }

    [Fact]
    public void SingleEdgeExcludesSinkFromSources()
    {
        var edges = new[]
        {
            ("a", "b")
        }.Select(x => x.ToTuple());
        var testee = Dag<string, Unit>.FromUntypedEdges(edges);

        testee.Sources.Should().BeEquivalentTo(new[] { "a" });
    }

    [Fact]
    public void SingleEdgeExcludesSourceFromSinks()
    {
        var edges = new[]
        {
            ("a", "b")
        }.Select(x => x.ToTuple());
        var testee = Dag<string, Unit>.FromUntypedEdges(edges);

        testee.Sinks.Should().BeEquivalentTo(new[] { "b" });
    }

    [Fact]
    public void MoreComplexSources()
    {
        /*

         a -> b -> c
              ^
             /
            /
           d

         */
        var edges = new[]
        {
            ("a", "b"),
            ("b", "c"),
            ("d", "b"),
        }.Select(x => x.ToTuple());
        var testee = Dag<string, Unit>.FromUntypedEdges(edges);

        testee.Sources.Should().BeEquivalentTo(new[] { "a", "d" });
    }

    [Fact]
    public void MoreComplexSinks()
    {
        /*

         a -> b -> c
              \
               \
                v
                 d

         */
        var edges = new[]
        {
            ("a", "b"),
            ("b", "c"),
            ("b", "d"),
        }.Select(x => x.ToTuple());
        var testee = Dag<string, Unit>.FromUntypedEdges(edges);

        testee.Sinks.Should().BeEquivalentTo(new[] { "c", "d" });
    }
}
