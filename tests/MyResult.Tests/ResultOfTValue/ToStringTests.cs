namespace MyResult.Tests.ResultOfTValue;

public sealed class ToStringTests
{
    [Fact]
    public void ToString_ResultIsSuccess_ReturnsCorrectString()
    {
        // Arrange
        var result = Result<int>.Ok(5);

        // Act
        var str = result.ToString();

        // Assert
        Assert.Equal("Result { IsSuccess = True, Value = 5 }", str);
    }

    [Fact]
    public void ToString_ResultIsFailure_ReturnsCorrectString()
    {
        // Arrange
        var result = Result<int>.Fail(new MyResult.Error("code", "description"));

        // Act
        var str = result.ToString();

        // Assert
        Assert.Equal("Result { IsSuccess = False, Error = Error { Code = code, Description = description } }", str);
    }
}