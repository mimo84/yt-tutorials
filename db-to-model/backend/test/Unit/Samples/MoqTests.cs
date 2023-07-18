using FluentAssertions;
using Moq;
using Xunit;

namespace Unit.Samples;

public interface IFoo
{
    Bar Bar { get; set; }
    string Name { get; set; }
    int Value { get; set; }
    bool DoSomething(string value);
    bool DoSomething(int number, string value);
    Task<bool> DoSomethingAsync();
    string DoSomethingStringy(string value);
    bool TryParse(string value, out string outputValue);
    bool Submit(ref Bar bar);
    int GetCount();
    bool Add(int value);
}

public class Bar
{
    public virtual Baz Baz { get; set; }

    public virtual bool Submit()
    {
        return false;
    }
}

public class Baz
{
    public virtual string Name { get; set; }
}

public class MoqTests
{
    [Fact]
    public void MoqExampleTest()
    {
        var mock = new Mock<IFoo>();
        mock.Setup(foo => foo.DoSomething("ping")).Returns(true);

        mock.Object.DoSomething("ping").Should().BeTrue();
    }

    [Fact]
    public void MoqExampleOutStringTest()
    {
        var mock = new Mock<IFoo>();

        // out arguments
        var outString = "ack";
        // TryParse will return true, and the out argument will return "ack", lazy evaluated
        mock.Setup(foo => foo.TryParse("ping", out outString)).Returns(true);

        string expectedOutString = string.Empty;
        mock.Object.TryParse("ping", out expectedOutString).Should().BeTrue();
    }

    [Fact]
    public void MoqRefArgsTest()
    {
        var mock = new Mock<IFoo>();

        // ref arguments
        var instance = new Bar();
        // Only matches if the ref argument to the invocation is the same instance
        mock.Setup(foo => foo.Submit(ref instance)).Returns(true);

        mock.Object.Submit(ref instance).Should().BeTrue();
    }

    [Fact]
    public void MoqInvokeMockTest()
    {
        var mock = new Mock<IFoo>();

        // access invocation arguments when returning a value
        mock.Setup(x => x.DoSomethingStringy(It.IsAny<string>()))
            .Returns((string s) => s.ToLower());

        mock.Object.DoSomethingStringy("WhatEVeR").Should().Be("whatever");
    }

    [Fact]
    public void MoqInvokeSpecificParamsTest()
    {
        var mock = new Mock<IFoo>();

        // throwing when invoked with specific parameters
        mock.Setup(foo => foo.DoSomething("reset")).Throws<InvalidOperationException>();
        mock.Setup(foo => foo.DoSomething("")).Throws(new ArgumentException("command"));

        Action act = () => mock.Object.DoSomething("reset");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Operation is not valid due to the current state of the object.");
    }

    [Fact]
    public void MoqLazyEvaluationTestExample()
    {
        var mock = new Mock<IFoo>();

        // lazy evaluating return value
        var count = 3;
        mock.Setup(foo => foo.GetCount()).Returns(() => count);
        mock.Object.GetCount().Should().Be(3);
    }

    [Fact]
    public async void MoqAsyncMethodTestExample()
    {
        var mock = new Mock<IFoo>();
        // async methods:
        mock.Setup(foo => foo.DoSomethingAsync().Result).Returns(true);

        var result = await mock.Object.DoSomethingAsync();
        result.Should().BeTrue();
    }
}
