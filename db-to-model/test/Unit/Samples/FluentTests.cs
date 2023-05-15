using FluentAssertions;
using Xunit;

namespace Unit.Samples;

public class FluentTests
{
    [Fact]
    public void FluentSimpleAssertion()
    {
        string actual = "MostlyCode";
        actual.Should().StartWith("M").And.EndWith("Code").And.Contain("ly").And.HaveLength(10);
    }

    [Fact]
    public void FluentListAssertion()
    {
        IList<int> collection = new[] { 1, 2, 3, 4 };

        collection.Should().OnlyContain(n => n > 0, "we only want positive numbers");
        collection.Should().HaveCount(4, "we should only have four elements in the list");

    }
}
