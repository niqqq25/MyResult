namespace MyResult.Tests.Result;

public sealed class IsSuccessTests
{
    [Fact]
    public void IsSuccess_CreatedWithStaticFactoryMethodOk_ReturnsTrue()
    {
        // Arrange
        var result = MyResult.Result.Ok();

        // Act
        var isSuccess = result.IsSuccess;

        // Assert
        Assert.True(isSuccess);
    }

    [Fact]
    public void IsSuccess_CreatedWithStaticFactoryMethodFail_ReturnsFalse()
    {
        // Arrange
        var result = MyResult.Result.Fail(new MyResult.Error("Code", "Message"));

        // Act
        var isSuccess = result.IsSuccess;

        // Assert
        Assert.False(isSuccess);
    }

    [Fact]
    public void IsSuccess_CreatedWithImplicitConversionFail_ReturnsFalse()
    {
        // Arrange
        MyResult.Result result = new MyResult.Error("Code", "Message");

        // Act
        var isSuccess = result.IsSuccess;

        // Assert
        Assert.False(isSuccess);
    }
}