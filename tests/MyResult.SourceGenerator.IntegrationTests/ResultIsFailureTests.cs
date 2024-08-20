namespace MyResult.SourceGenerator.IntegrationTests;

public sealed class ResultIsFailureTests
{
    [Fact]
    public void IsFailure_ResultWithStructErrorIsFailure_ReturnsTrue()
    {
        var result = ResultWithStructError.Fail(new StructError("Oops"));
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void IsFailure_ResultWithStructErrorIsSuccess_ReturnsFalse()
    {
        var result = ResultWithStructError.Ok();
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void IsFailure_ResultWithReferenceTypeErrorIsFailure_ReturnsTrue()
    {
        var result = ClassResult.Fail(new Error("Oops"));
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void IsFailure_ResultWithReferenceTypeErrorIsSuccess_ReturnsFalse()
    {
        var result = ClassResult.Ok();
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void IsFailure_ResultWithValueTypeErrorIsFailure_ReturnsTrue()
    {
        var result = ClassResultOfTValueTError<string, int>.Fail(2);
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void IsFailure_ResultWithValueTypeErrorIsSuccess_ReturnsFalse()
    {
        var result = ClassResultOfTValueTError<string, int>.Ok("");
        Assert.False(result.IsFailure);
    }
}

public readonly record struct StructError(string Name);

[Result(typeof(StructError))]
public readonly partial struct ResultWithStructError;