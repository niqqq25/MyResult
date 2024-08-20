namespace MyResult.Tests.ResultOfTValueTError;

public sealed class ErrorPropertyTests
{
    [Fact]
    public void Error_CreatedWithStaticFactoryMethodFail_ReturnsError()
    {
        // Arrange
        const string expectedError = "Error";
        var result = Result<int, string>.Fail(expectedError);

        // Act
        var actualError = result.Error;

        // Assert
        Assert.Equal(expectedError, actualError);
    }

    [Fact]
    public void Error_CreatedWithImplicitConversionFail_ReturnsError()
    {
        // Arrange
        const string expectedError = "Error";
        Result<int, string> result = expectedError;

        // Act
        var actualError = result.Error;

        // Assert
        Assert.Equal(expectedError, actualError);
    }

    [Fact]
    public void Error_CreatedWithStaticFactoryMethodOk_ThrowsException()
    {
        // Arrange
        var result = Result<int, string>.Ok(20);

        // Act
        var getError = () => result.Error;

        // Assert
        Assert.Throws<InvalidOperationException>(getError);
    }

    [Fact]
    public void Error_CreatedWithImplicitConversionOk_ThrowsException()
    {
        // Arrange
        Result<int, string> result = 20;

        // Act
        var getError = () => result.Error;

        // Assert
        Assert.Throws<InvalidOperationException>(getError);
    }
}