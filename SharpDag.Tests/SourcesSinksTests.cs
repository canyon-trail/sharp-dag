using SharpDag.CSharp;

namespace SharpDag.Tests;

public sealed class SourcesSinksTests
{
    [Fact]
    public void TreatsIsolatedNodesAsSources()
    {
        var testee = Dag.FromNodes(new[] { "a", "b", "c" });

        testee.Sources.Should().BeEquivalentTo(new[] { "a", "b", "c" });
    }

    [Fact]
    public void TreatsIsolatedNodesAsSinks()
    {
        var testee = Dag.FromNodes(new[] { "a", "b", "c" });

        testee.Sinks.Should().BeEquivalentTo(new[] { "a", "b", "c" });
    }

    [Fact]
    public void SingleEdgeExcludesSinkFromSources()
    {
        var edges = new[]
        {
            Edge.Untyped("a", "b")
        };
        var testee = Dag.FromUntypedEdges(edges);

        testee.Sources.Should().BeEquivalentTo(new[] { "a" });
    }

    [Fact]
    public void SingleEdgeExcludesSourceFromSinks()
    {
        var edges = new[]
        {
            Edge.Untyped("a", "b")
        };
        var testee = Dag.FromUntypedEdges(edges);

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
            Edge.Untyped("a", "b"),
            Edge.Untyped("b", "c"),
            Edge.Untyped("d", "b"),
        };
        var testee = Dag.FromUntypedEdges(edges);

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
            Edge.Untyped("a", "b"),
            Edge.Untyped("b", "c"),
            Edge.Untyped("b", "d"),
        };
        var testee = Dag.FromUntypedEdges(edges);

        testee.Sinks.Should().BeEquivalentTo(new[] { "c", "d" });
    }
}
