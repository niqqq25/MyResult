namespace MyResult.Tests.Result;

public sealed class ToStringTests
{
    [Fact]
    public void ToString_ResultIsSuccess_ReturnsCorrectString()
    {
        // Arrange
        var result = MyResult.Result.Ok();

        // Act
        var str = result.ToString();

        // Assert
        Assert.Equal("Result { IsSuccess = True }", str);
    }

    [Fact]
    public void ToString_ResultIsFailure_ReturnsCorrectString()
    {
        // Arrange
        var result = MyResult.Result.Fail(new MyResult.Error("code", "description"));

        // Act
        var str = result.ToString();

        // Assert
        Assert.Equal("Result { IsSuccess = False, Error = Error { Code = code, Description = description } }", str);
    }
}