namespace SharpDag.Tests;

public class NodeListTests
{
    [Fact]
    public void AcceptsSingleNode()
    {
        var testee = Dag.fromNodes(new[] { "a" });

        testee.Nodes.Should().BeEquivalentTo(new[] { "a" });
    }

    [Fact]
    public void AcceptsMultipleNodes()
    {
        var testee = Dag.fromNodes(new[] { "a", "b" });

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
            ("a", "b"),
            ("b", "c"),
            ("c", "d"),
        }
            .Select(x => x.ToTuple());
        var testee = Dag.fromEdges(edges);

        testee.Nodes.Should().BeEquivalentTo(new[] { "a", "b", "c", "d" });
    }
}
