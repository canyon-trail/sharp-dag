namespace SharpDag.Tests;

using SharpDag.CSharp;

public class NodeListTests
{
    [Fact]
    public void AcceptsSingleNode()
    {
        var testee = Dag.FromNodes(new[] { "a" });

        testee.Nodes.Should().BeEquivalentTo(new[] { "a" });
    }

    [Fact]
    public void AcceptsMultipleNodes()
    {
        var testee = Dag.FromNodes(new[] { "a", "b" });

        testee.Nodes.Should().BeEquivalentTo(new[] { "a", "b" });
    }

    [Fact]
    public void AcceptsLinearGraph()
    {
        /*
            a -> b -> c -> d
         */
        var edges = new[]
        {
            Edge.Untyped("a", "b"),
            Edge.Untyped("b", "c"),
            Edge.Untyped("c", "d"),
        };
        var testee = Dag.FromUntypedEdges(edges);

        testee.Nodes.Should().BeEquivalentTo(new[] { "a", "b", "c", "d" });
    }
}
