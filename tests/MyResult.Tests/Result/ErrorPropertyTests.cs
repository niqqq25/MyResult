namespace MyResult.Tests.Result;

public sealed class ErrorPropertyTests
{
    [Fact]
    public void Error_CreatedWithStaticFactoryMethodFail_ReturnsError()
    {
        // Arrange
        var expectedError = new MyResult.Error("Code", "Message");
        var result = MyResult.Result.Fail(expectedError);

        // Act
        var actualError = result.Error;

        // Assert
        Assert.Equal(expectedError, actualError);
    }

    [Fact]
    public void Error_CreatedWithStaticFactoryMethodOk_ThrowsException()
    {
        // Arrange
        var result = MyResult.Result.Ok();

        // Act
        var getError = () => result.Error;

        // Assert
        Assert.Throws<InvalidOperationException>(getError);
    }

    [Fact]
    public void Error_CreatedWithImplicitConversionFail_ReturnsError()
    {
        // Arrange
        var expectedError = new MyResult.Error("Code", "Message");
        MyResult.Result result = expectedError;

        // Act
        var actualError = result.Error;

        // Assert
        Assert.Equal(expectedError, actualError);
    }
}