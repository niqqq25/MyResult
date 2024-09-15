namespace MyResult.Tests.ResultOfTValue;

public sealed class ToStringTests
{
    [Fact]
    public void ToString_ResultIsSuccess_ReturnsCorrectString()
    {
        // Arrange
        const int value = 5;
        var result = Result<int>.Ok(value);

        // Act
        var str = result.ToString();

        // Assert
        Assert.Equal($$"""Result { IsSuccess = True, Value = {{value}} }""", str);
    }

    [Fact]
    public void ToString_ResultIsFailure_ReturnsCorrectString()
    {
        // Arrange
        var error = new MyResult.Error("code", "description");
        var result = Result<int>.Fail(error);

        // Act
        var str = result.ToString();

        // Assert
        Assert.Equal($$"""Result { IsSuccess = False, Error = {{error}} }""", str);
    }
}