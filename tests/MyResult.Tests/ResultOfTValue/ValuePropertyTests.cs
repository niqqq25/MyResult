namespace MyResult.Tests.ResultOfTValue;

public sealed class ValuePropertyTests
{
    [Fact]
    public void Value_CreatedWithStaticFactoryMethodOk_ReturnsValue()
    {
        // Arrange
        const int expectedValue = 20;
        var result = Result<int>.Ok(expectedValue);

        // Act
        var actualValue = result.Value;

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void Value_CreatedWithImplicitConversionOk_ReturnsValue()
    {
        // Arrange
        const int expectedValue = 20;
        Result<int> result = expectedValue;

        // Act
        var actualValue = result.Value;

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void Value_CreatedWithStaticFactoryMethodFail_ThrowsException()
    {
        // Arrange
        var result = Result<int>.Fail(new MyResult.Error("Code", "Message"));

        // Act
        var getValue = () => (object)result.Value;

        // Assert
        Assert.Throws<InvalidOperationException>(getValue);
    }

    [Fact]
    public void Value_CreatedWithImplicitConversionFail_ThrowsException()
    {
        // Arrange
        Result<int> result = new MyResult.Error("Code", "Message");

        // Act
        var getValue = () => (object)result.Value;

        // Assert
        Assert.Throws<InvalidOperationException>(getValue);
    }
}