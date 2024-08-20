using Microsoft.CSharp.RuntimeBinder;

namespace MyResult.SourceGenerator.IntegrationTests;

public sealed class ResultImplicitOperatorsTests
{
    [Fact]
    public void ImplicitConversion_HasImplicitConversion_ConvertsValueAndError()
    {
        // Arrange
        const int value = 2;
        const string error = "oops";

        // Act
        ClassResultOfTValueTError<int, string> resultWithValue = value;
        ClassResultOfTValueTError<int, string> resultWithError = error;

        // Arrange
        Assert.Equal(value, resultWithValue.Value);
        Assert.Equal(error, resultWithError.Error);
    }

    [Fact]
    public void ImplicitConversion_DoesNotHaveImplicitConversion_ThrowsRuntimeBinderException()
    {
        // Arrange
        const int value = 2;
        const string error = "oops";

        // Act
        Func<ResultWithoutImplicitOperators<int, string>> implicitValueConversion = () => (dynamic)value;
        Func<ResultWithoutImplicitOperators<int, string>> implicitErrorConversion = () => (dynamic)error;

        // Assert
        Assert.Throws<RuntimeBinderException>(implicitValueConversion);
        Assert.Throws<RuntimeBinderException>(implicitErrorConversion);
    }
}

[Result(hasImplicitConversion: false)]
public partial record ResultWithoutImplicitOperators<TValue, TError>;