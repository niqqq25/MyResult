namespace MyResult.Tests.Error;

internal sealed class DerivedError(
    string code,
    string description,
    string extraField) : MyResult.Error(code, description)
{
    public string ExtraField { get; } = extraField;
}