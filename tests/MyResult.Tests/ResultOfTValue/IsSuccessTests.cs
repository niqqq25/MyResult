namespace MyResult.Tests.ResultOfTValue;

public sealed class IsSuccessTests
{
    [Fact]
    public void IsSuccess_CreatedWithStaticFactoryMethodOk_ReturnsTrue()
    {
        // Arrange
        var result = Result<int>.Ok(30);

        // Act
        var isSuccess = result.IsSuccess;

        // Assert
        Assert.True(isSuccess);
    }

    [Fact]
    public void IsSuccess_CreatedWithImplicitConversionOk_ReturnsTrue()
    {
        // Arrange
        Result<int> result = 20;

        // Act
        var isFailure = result.IsSuccess;

        // Assert
        Assert.True(isFailure);
    }

    [Fact]
    public void IsSuccess_CreatedWithStaticFactoryMethodFail_ReturnsFalse()
    {
        // Arrange
        var result = Result<int>.Fail(new MyResult.Error("Code", "Message"));

        // Act
        var isSuccess = result.IsSuccess;

        // Assert
        Assert.False(isSuccess);
    }

    [Fact]
    public void IsSuccess_CreatedWithImplicitConversionFail_ReturnsFalse()
    {
        // Arrange
        Result<int> result = new MyResult.Error("Code", "Message");

        // Act
        var isFailure = result.IsSuccess;

        // Assert
        Assert.False(isFailure);
    }
}