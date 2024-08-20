namespace MyResult.Tests.Result;

public sealed class IsFailureTests
{
    [Fact]
    public void IsFailure_CreatedWithStaticFactoryMethodFail_ReturnsTrue()
    {
        // Arrange
        var result = MyResult.Result.Fail(new MyResult.Error("Code", "Message"));

        // Act
        var isFailure = result.IsFailure;

        // Assert
        Assert.True(isFailure);
    }

    [Fact]
    public void IsFailure_CreatedWithStaticFactoryMethodOk_ReturnsFalse()
    {
        // Arrange
        var result = MyResult.Result.Ok();

        // Act
        var isFailure = result.IsFailure;

        // Assert
        Assert.False(isFailure);
    }

    [Fact]
    public void IsFailure_CreatedWithImplicitConversionFail_ReturnsTrue()
    {
        // Arrange
        MyResult.Result result = new MyResult.Error("Code", "Message");

        // Act
        var isFailure = result.IsFailure;

        // Assert
        Assert.True(isFailure);
    }
}