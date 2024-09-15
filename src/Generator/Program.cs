using System.Reflection;
using MyResult.SourceGenerator;
using MyResult.SourceGenerator.Templates;
using ValueType = MyResult.SourceGenerator.ValueType;

const string name = "Result";
const string @namespace = "MyResult";
const TypeSymbol symbol = TypeSymbol.RecordStruct;
const string modifiers = "public readonly";

var sourceRoot = Path.GetFullPath(
    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "..", "..", "..", ".."));

var resultContext = new ResultContext(
    name,
    symbol,
    modifiers,
    @namespace,
    new ErrorType("Error", false, false),
    valueType: null,
    hasToStringOverride: false,
    hasImplicitConversion: true,
    isSerializable: true);

const string resultAdditionalCode = """
                                   public static Result<TValue> Ok<TValue>(TValue value)
                                   {
                                       return Result<TValue>.Ok(value);
                                   }

                                   public static Result<TValue> Fail<TValue>(Error error)
                                   {
                                       return Result<TValue>.Fail(error);
                                   }

                                   public static Result<TValue, TError> Ok<TValue, TError>(TValue value)
                                   {
                                       return Result<TValue, TError>.Ok(value);
                                   }

                                   public static Result<TValue, TError> Fail<TValue, TError>(TError error)
                                   {
                                       return Result<TValue, TError>.Fail(error);
                                   }
                           """;

File.WriteAllText(
    Path.Combine(sourceRoot, "MyResult", $"{name}.cs"),
    ResultTemplate.Generate(resultContext, true, "IResult", resultAdditionalCode));

var resultOfTValueContext = new ResultContext(
    name,
    symbol,
    modifiers,
    @namespace,
    new ErrorType("Error", false, false),
    new ValueType("TValue"),
    hasToStringOverride: false,
    hasImplicitConversion: true,
    isSerializable: true);

File.WriteAllText(
    Path.Combine(sourceRoot, "MyResult", $"{name}`1.cs"),
    ResultTemplate.Generate(resultOfTValueContext, true, "IResult<TValue>"));

var resultOfTValueTErrorContext = new ResultContext(
    name,
    symbol,
    modifiers,
    @namespace,
    new ErrorType("TError", false, true),
    new ValueType("TValue"),
    hasToStringOverride: false,
    hasImplicitConversion: true,
    isSerializable: true);

File.WriteAllText(
    Path.Combine(sourceRoot, "MyResult", $"{name}`2.cs"),
    ResultTemplate.Generate(resultOfTValueTErrorContext, true));
