namespace MyResult.Tests.ResultOfTValue;

public sealed class IsFailureTests
{
    [Fact]
    public void IsFailure_CreatedWithStaticFactoryMethodFail_ReturnsTrue()
    {
        // Arrange
        var result = Result<int>.Fail(new MyResult.Error("Code", "Message"));

        // Act
        var isFailure = result.IsFailure;

        // Assert
        Assert.True(isFailure);
    }

    [Fact]
    public void IsFailure_CreatedWithImplicitConversionFail_ReturnsTrue()
    {
        // Arrange
        Result<int> result = new MyResult.Error("Code", "Message");

        // Act
        var isFailure = result.IsFailure;

        // Assert
        Assert.True(isFailure);
    }

    [Fact]
    public void IsFailure_CreatedWithStaticFactoryMethodOk_ReturnsFalse()
    {
        // Arrange
        var result = Result<int>.Ok(20);

        // Act
        var isFailure = result.IsFailure;

        // Assert
        Assert.False(isFailure);
    }

    [Fact]
    public void IsFailure_CreatedWithImplicitConversionOk_ReturnsFalse()
    {
        // Arrange
        Result<int> result = 20;

        // Act
        var isFailure = result.IsFailure;

        // Assert
        Assert.False(isFailure);
    }
}