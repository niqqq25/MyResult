namespace MyResult.SourceGenerator.IntegrationTests;

public sealed class ResultToStringTests
{
    [Fact]
    public void ToString_NotOverridenAndSuccessWithoutValue_ReturnsDefaultString()
    {
        // Arrange
        var result = ClassResult.Ok();

        // Act
        var resultAsString = result.ToString();

        // Assert
        Assert.Equal("ClassResult { IsSuccess = True }", resultAsString);
    }

    [Fact]
    public void ToString_NotOverridenAndFailure_ReturnsDefaultString()
    {
        // Arrange
        var result = ClassResultOfTValueTError<int, string>.Fail("Oops");

        // Act
        var resultAsString = result.ToString();

        // Assert
        Assert.Equal("ClassResultOfTValueTError { IsSuccess = False, Error = Oops }", resultAsString);
    }

    [Fact]
    public void ToString_NotOverridenAndSuccessWithValue_ReturnsDefaultString()
    {
        // Arrange
        var result = ClassResultOfTValue<int>.Ok(2);

        // Act
        var resultAsString = result.ToString();

        // Assert
        Assert.Equal("ClassResultOfTValue { IsSuccess = True, Value = 2 }", resultAsString);
    }

    [Fact]
    public void ToString_Overriden_ReturnsOverridenString()
    {
        // Arrange
        var result = ResultWithOverridenToString<int, string>.Ok(2);

        // Act
        var resultAsString = result.ToString();

        // Assert
        Assert.Equal("Overriden", resultAsString);
    }
}

[Result]
public partial record ResultWithOverridenToString<TValue, TError>
{
    public override string ToString()
    {
        return "Overriden";
    }
}