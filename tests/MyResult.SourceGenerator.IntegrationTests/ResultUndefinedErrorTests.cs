namespace MyResult.SourceGenerator.IntegrationTests;

public sealed class ResultUndefinedErrorTests
{
    [Fact]
    public void GetErrorType_ResultErrorTypeNotProvided_ErrorTypeIsUndefinedError()
    {
        // Arrange
        var result = ResultWithUndefinedError.Fail(new UndefinedError());
        var resultOfTValue = ResultWithUndefinedErrorOfTValue<int>.Fail(new UndefinedError());

        // Act

        var errorType = result.Error!.GetType();
        var errorTypeOfTValue = resultOfTValue.Error!.GetType();

        // Assert
        Assert.Equal(typeof(UndefinedError), errorType);
        Assert.Equal(typeof(UndefinedError), errorTypeOfTValue);
    }
}

[Result]
public sealed partial class ResultWithUndefinedError;

[Result]
public sealed partial class ResultWithUndefinedErrorOfTValue<TValue>;