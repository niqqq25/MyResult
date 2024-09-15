namespace MyResult.SourceGenerator;

internal readonly record struct ResultContext // TODO
{
    public ResultContext(
        string name,
        TypeSymbol typeSymbol,
        string modifiers,
        string? @namespace,
        ErrorType errorType,
        ValueType? valueType,
        bool hasToStringOverride,
        bool hasImplicitConversion,
        bool isSerializable)
    {
        Name = name;
        TypeSymbol = typeSymbol;
        Modifiers = modifiers;
        Namespace = @namespace;
        ErrorType = errorType;
        ValueType = valueType;
        HasToStringOverride = hasToStringOverride;
        HasImplicitConversion = hasImplicitConversion;
        IsSerializable = isSerializable;
    }

    ///<summary></summary>
    public string Name { get; } // TODO add summary

    public TypeSymbol TypeSymbol { get; }

    public string Modifiers { get; }

    // TODO add summary. Null when namescape is global
    ///<summary></summary>
    public string? Namespace { get; }

    public ErrorType ErrorType { get; }

    public ValueType? ValueType { get; }

    public bool HasToStringOverride { get; }

    public bool HasImplicitConversion { get; }
    
    public bool IsSerializable { get; }
}

internal readonly record struct ErrorType(string Name, bool IsInterface, bool IsGeneric)
{
    public string Name { get; } = Name;

    public bool IsInterface { get; } = IsInterface;

    public bool IsGeneric { get; } = IsGeneric;
}

internal readonly record struct ValueType(string Name)
{
    public string Name { get; } = Name;
}

internal enum TypeSymbol
{
    Class = 1,
    Struct,
    Record,
    RecordStruct
}