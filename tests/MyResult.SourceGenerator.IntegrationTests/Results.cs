namespace MyResult.SourceGenerator.IntegrationTests;

public record Error(string Message);

public record DerivedError(string Message, string ExtraField) : Error(Message);

[Result(typeof(Error), isSerializable: true)]
public partial class ClassResult
{
}

[Result(typeof(Error), isSerializable: true)]
public partial class ClassResultOfTValue<TValue>
{
}

[Result(isSerializable: true)]
public partial class ClassResultOfTValueTError<TValue, TError>
{
}

[Result(typeof(Error))]
public partial record RecordResult
{
}

[Result(typeof(Error))]
public partial record RecordResultOfTValue<TValue>
{
}

[Result]
public partial record RecordResultOfTValueTError<TValue, TError>
{
}

[Result(typeof(Error))]
public readonly partial struct ReadonlyStructResult
{
}

[Result(typeof(Error))]
public readonly partial struct ReadonlyStructResultOfTValue<TValue>
{
}

[Result]
public readonly partial struct ReadonlyStructResultOfTValueTError<TValue, TError>
{
}

[Result(typeof(Error))]
public readonly partial record struct ReadonlyRecordStructResult
{
}

[Result(typeof(Error))]
public readonly partial record struct ReadonlyRecordStructResultOfTValue<TValue>
{
}

[Result]
public readonly partial record struct ReadonlyRecordStructResultOfTValueTError<TValue, TError>
{
}

