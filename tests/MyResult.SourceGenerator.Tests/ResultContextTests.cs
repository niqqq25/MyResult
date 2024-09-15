namespace MyResult.SourceGenerator.Tests;

public sealed class ResultContextTests
{
    [Fact]
    public void Equals_TwoIdenticalInstances_ReturnsTrue()
    {
        var instance1 = GetResultContext();
        var instance2 = GetResultContext();

        Assert.Equal(instance1, instance2);
        Assert.True(instance1.Equals(instance2));
        Assert.True(instance1 == instance2);

        ResultContext GetResultContext() =>
            new(
                name: "MyStruct",
                typeSymbol: TypeSymbol.Struct,
                modifiers: "partial readonly",
                "MyNamespace",
                errorType: new ErrorType("MyError", false, true),
                valueType: new ValueType("Hello world"),
                hasToStringOverride: true,
                hasImplicitConversion: true,
                isSerializable: true);
    }

    [Fact]
    public void Equals_TwoDifferentInstances_ReturnsFalse()
    {
        var instance1 = new ResultContext (
            name: "MyStruct",
            typeSymbol: TypeSymbol.Struct,
            modifiers: "partial readonly",
            "MyNamespace",
            errorType: new ErrorType("MyError", false, true),
            valueType: new ValueType("Hello world"),
            hasToStringOverride: true,
            hasImplicitConversion: true,
            isSerializable: true);

        var instance2 = new ResultContext (
            name: "MyStruct",
            typeSymbol: TypeSymbol.Struct,
            modifiers: "partial readonly",
            "MyNamespace",
            errorType: new ErrorType("MyErrorr", false, true),
            valueType: new ValueType("Hello world"),
            hasToStringOverride: true,
            hasImplicitConversion: true,
            isSerializable: true);

        Assert.NotEqual(instance1, instance2);
        Assert.False(instance1.Equals(instance2));
        Assert.False(instance1 == instance2);
    }
}