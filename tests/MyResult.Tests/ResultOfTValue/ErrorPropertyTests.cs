namespace MyResult.Tests.ResultOfTValue;

public sealed class ErrorPropertyTests
{
    [Fact]
    public void Error_CreatedWithStaticFactoryMethodFail_ReturnsError()
    {
        // Arrange
        var expectedError = new MyResult.Error("Code", "Message");
        var result = Result<int>.Fail(expectedError);

        // Act
        var actualError = result.Error;

        // Assert
        Assert.Equal(expectedError, actualError);
    }

    [Fact]
    public void Error_CreatedWithImplicitConversionFail_ReturnsError()
    {
        // Arrange
        var expectedError = new MyResult.Error("Code", "Message");
        Result<int> result = expectedError;

        // Act
        var actualError = result.Error;

        // Assert
        Assert.Equal(expectedError, actualError);
    }

    [Fact]
    public void Error_CreatedWithStaticFactoryMethodOk_ThrowsException()
    {
        // Arrange
        var result = Result<int>.Ok(20);

        // Act
        var getError = () => result.Error;

        // Assert
        Assert.Throws<InvalidOperationException>(getError);
    }

    [Fact]
    public void Error_CreatedWithImplicitConversionOk_ThrowsException()
    {
        // Arrange
        Result<int> result = 20;

        // Act
        var getError = () => result.Error;

        // Assert
        Assert.Throws<InvalidOperationException>(getError);
    }
}