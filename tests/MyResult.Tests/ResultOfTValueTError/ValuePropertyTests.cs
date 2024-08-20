namespace MyResult.Tests.ResultOfTValueTError;

public sealed class ValuePropertyTests
{
    [Fact]
    public void Value_CreatedWithStaticFactoryMethodOk_ReturnsValue()
    {
        // Arrange
        const int expectedValue = 20;
        var result = Result<int, string>.Ok(expectedValue);

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
        Result<int, string> result = expectedValue;

        // Act
        var actualValue = result.Value;

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void Value_CreatedWithStaticFactoryMethodFail_ThrowsException()
    {
        // Arrange
        var result = Result<int, string>.Fail("Error");

        // Act
        var getValue = () => (object)result.Value;

        // Assert
        Assert.Throws<InvalidOperationException>(getValue);
    }

    [Fact]
    public void Value_CreatedWithImplicitConversionFail_ThrowsException()
    {
        // Arrange
        Result<int, string> result = "Error";

        // Act
        var getValue = () => (object)result.Value;

        // Assert
        Assert.Throws<InvalidOperationException>(getValue);
    }
}