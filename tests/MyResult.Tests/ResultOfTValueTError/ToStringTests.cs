namespace MyResult.Tests.ResultOfTValueTError;

public sealed class ToStringTests
{
    [Fact]
    public void ToString_ResultIsSuccess_ReturnsCorrectString()
    {
        // Arrange
        const int value = 5;
        var result = Result<int, string>.Ok(value);

        // Act
        var str = result.ToString();

        // Assert
        Assert.Equal($$"""Result { IsSuccess = True, Value = {{value}} }""", str);
    }

    [Fact]
    public void ToString_ResultIsFailure_ReturnsCorrectString()
    {
        // Arrange
        const string error = "Error";
        var result = Result<int, string>.Fail(error);

        // Act
        var str = result.ToString();

        // Assert
        Assert.Equal($$"""Result { IsSuccess = False, Error = {{error}} }""", str);
    }
}