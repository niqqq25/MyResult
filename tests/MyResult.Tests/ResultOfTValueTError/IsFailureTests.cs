namespace MyResult.Tests.ResultOfTValueTError;

public sealed class IsFailureTests
{
    [Fact]
    public void IsFailure_CreatedWithStaticFactoryMethodFail_ReturnsTrue()
    {
        // Arrange
        var result = Result<int, string>.Fail("Error");

        // Act
        var isFailure = result.IsFailure;

        // Assert
        Assert.True(isFailure);
    }

    [Fact]
    public void IsFailure_CreatedWithImplicitConversionFail_ReturnsTrue()
    {
        // Arrange
        Result<int, string> result = "Error";

        // Act
        var isFailure = result.IsFailure;

        // Assert
        Assert.True(isFailure);
    }

    [Fact]
    public void IsFailure_CreatedWithStaticFactoryMethodOk_ReturnsFalse()
    {
        // Arrange
        var result = Result<int, string>.Ok(20);

        // Act
        var isFailure = result.IsFailure;

        // Assert
        Assert.False(isFailure);
    }

    [Fact]
    public void IsFailure_CreatedWithImplicitConversionOk_ReturnsFalse()
    {
        // Arrange
        Result<int, string> result = 20;

        // Act
        var isFailure = result.IsFailure;

        // Assert
        Assert.False(isFailure);
    }
}