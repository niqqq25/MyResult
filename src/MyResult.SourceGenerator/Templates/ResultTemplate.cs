﻿using System.Text;

namespace MyResult.SourceGenerator.Templates;

internal static class ResultTemplate
{
    private const string Header = """
                                  //------------------------------------------------------------------------------
                                  // <auto-generated>
                                  //     This code was generated by the MyResult source generator.
                                  //
                                  //     Changes to this file may cause incorrect behavior and will be lost if
                                  //     the code is regenerated.
                                  // </auto-generated>
                                  //------------------------------------------------------------------------------

                                  """;

    public static string Generate(
        ResultContext context,
        bool shouldRemoveHeader = false,
        string? additionalInterfaces = null,
        string? additionalCode = null)
    {
        var nameWithGenericTypeParameters = context.ErrorType.IsGeneric
            ? $"{context.Name}<{context.ValueType!.Value.Name}, {context.ErrorType.Name}>"
            : context.ValueType.HasValue
                ? $"{context.Name}<{context.ValueType.Value.Name}>"
                : context.Name;

        var interfaces = additionalInterfaces is null
            ? $"IEquatable<{nameWithGenericTypeParameters}>"
            : $"IEquatable<{nameWithGenericTypeParameters}>, {additionalInterfaces}";

        var sb = new StringBuilder(); // TODO add capacity

        if (shouldRemoveHeader is false)
        {
            sb.AppendLine(Header);   
        }

        sb.AppendLine("""
                      #pragma warning disable 8766 // nullability of reference types in return type doesn't match implicitly implemented member
                      
                      #nullable enable
                      
                      """);

        if (context.Namespace is not null)
        {
            sb.AppendLine($$"""
                            namespace {{context.Namespace}}
                            {
                            """);
        }

        sb.AppendLine("""
                          /// <summary>
                          /// Represents the result of an operation, which can either be a success or a failure.
                          /// </summary>
                      """);

        if (context.ValueType.HasValue)
        {
            sb.AppendLine($"""
                              /// <typeparam name="{context.ValueType.Value.Name}">The type of the value associated with the successful result.</typeparam>
                          """);
        }
        
        if (context.ErrorType.IsGeneric)
        {
            sb.AppendLine($"""
                               /// <typeparam name="{context.ErrorType.Name}">The type of the error associated with the failed result.</typeparam>
                           """);
        }

        if (context.IsSerializable)
        {
            var classNameWithGenerics = GetClassNameWithGenerics(context);
            var jsonConverter = context.ValueType.HasValue
                ? $"{classNameWithGenerics}JsonConverterFactory"
                : $"{context.Name}JsonConverter";
            sb.AppendLine($"    [System.Text.Json.Serialization.JsonConverter(typeof({jsonConverter}))]");
        }

        sb.AppendLine($$"""
                            {{context.Modifiers}} {{context.TypeSymbol.ToConstruct()}} {{nameWithGenericTypeParameters}} : {{interfaces}}
                            {
                                private readonly {{context.ErrorType.Name}}? _error;

                        """);

        if (context.ValueType.HasValue)
        {
            sb.AppendLine($"""
                                    private readonly {context.ValueType.Value.Name}? _value;

                            """);
        }

        sb.AppendLine($$"""
                                /// <summary>
                                /// Gets the error associated with the failed result.
                                /// </summary>
                                public {{context.ErrorType.Name}}? Error
                                {
                                    get
                                    {
                                        if (IsSuccess)
                                        {
                                            throw new System.InvalidOperationException($"{nameof({{nameWithGenericTypeParameters}})} is success. {nameof(Error)} is not set.");
                                        }
                        
                                        return _error;
                                    }
                                }

                        """);

        if (context.ValueType.HasValue)
        {
            sb.AppendLine($$"""
                                    /// <summary>
                                    /// Gets the value associated with the successful result.
                                    /// </summary>
                                    public TValue Value
                                    {
                                        get
                                        {
                                            if (IsFailure)
                                            {
                                                throw new System.InvalidOperationException($"{nameof({{nameWithGenericTypeParameters}})} is failure. {nameof(Value)} is not set.");
                                            }
                            
                                            return _value;
                                        }
                                    }
                            
                            """);
        }

        sb.AppendLine("""
                              /// <summary>
                              /// Checks whether the result represents a failed operation.
                              /// </summary>
                              /// <returns><c>true</c> if the operation failed; otherwise, <c>false</c>.</returns>
                      """);

        if (context.ValueType.HasValue)
        {
            sb.AppendLine("""
                                  [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(_value))]
                                  [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(Value))]
                          """);
        }

        sb.AppendLine("""
                              [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(_error))]
                              [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Error))]
                              public bool IsFailure => !IsSuccess;

                      """);

        sb.AppendLine("""
                              /// <summary>
                              /// Checks whether the result represents a successful operation.
                              /// </summary>
                              /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
                      """);

        if (context.ValueType.HasValue)
        {
            sb.AppendLine("""
                                  [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(_value))]
                                  [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Value))]
                          """);
        }

        sb.AppendLine("""
                              [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(_error))]
                              [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(Error))]
                              public bool IsSuccess { get; private init; }
                      
                      """);

        // static factory methods
        sb.AppendLine(context.ValueType.HasValue
            ? $$"""
                        /// <summary>Creates a successful result with the given value.</summary>
                        /// <param name="value">The value associated with the successful result.</param>
                        /// <returns>A new instance of <see cref="{{context.Name}}"/> representing a successful operation.</returns>
                        public static {{nameWithGenericTypeParameters}} Ok({{context.ValueType.Value.Name}} value)
                        {
                            return new {{nameWithGenericTypeParameters}}(value, default({{context.ErrorType.Name}}))
                            {
                                IsSuccess = true
                            };
                        }
                
                        /// <summary>Creates a failed result with the given error.</summary>
                        /// <param name="error">The error associated with the failed result.</param>
                        /// <returns>A new instance of <see cref="{{context.Name}}"/> representing a failed operation.</returns>
                        public static {{nameWithGenericTypeParameters}} Fail({{context.ErrorType.Name}} error)
                        {
                            return new {{nameWithGenericTypeParameters}}(default, error)
                            {
                                IsSuccess = false
                            };
                        }

                """
            : $$"""
                        /// <summary>Creates a successful result.</summary>
                        /// <returns>A new instance of <see cref="{{context.Name}}"/> representing a successful operation.</returns>
                        public static {{nameWithGenericTypeParameters}} Ok()
                        {
                            return new {{nameWithGenericTypeParameters}}(default({{context.ErrorType.Name}}))
                            {
                                IsSuccess = true
                            };
                        }

                        /// <summary>Creates a failed result with the given error.</summary>
                        /// <param name="error">The error associated with the failed result.</param>
                        /// <returns>A new instance of <see cref="{{context.Name}}"/> representing a failed operation.</returns>
                        public static {{nameWithGenericTypeParameters}} Fail({{context.ErrorType.Name}} error)
                        {
                            return new {{nameWithGenericTypeParameters}}(error)
                            {
                                IsSuccess = false
                            };
                        }

                """);

        // implicit operators
        if (context.HasImplicitConversion)
        {
            GenerateImplicitOperators(context, sb, nameWithGenericTypeParameters);
        }

        // constructors
        if (context.TypeSymbol is TypeSymbol.Struct or TypeSymbol.RecordStruct)
        {
            sb.AppendLine($$"""
                                    [System.Obsolete("Must not be used without parameters", true)]
                                    public {{context.Name}}()
                                    {
                                    }

                            """);
        }

        sb.AppendLine(context.ValueType.HasValue
            ? $$"""
                        private {{context.Name}}({{context.ValueType.Value.Name}}? value, {{context.ErrorType.Name}}? error)
                        {
                            _error = error;
                            _value = value;
                        }

                """
            : $$"""
                        private {{context.Name}}({{context.ErrorType.Name}}? error)
                        {
                            _error = error;
                        }

                """);

        // ToString method
        if (context.HasToStringOverride is false)
        {
            GenerateToStringMethod(context, sb);
        }

        // equality overrides
        if (context.TypeSymbol is TypeSymbol.Class or TypeSymbol.Struct)
        {
            GenerateEqualityOverrides(context, sb, nameWithGenericTypeParameters);
        }

        if (additionalCode is not null)
        {
            sb.AppendLine(additionalCode);
        }

        sb.AppendLine("    }");

        if (context.IsSerializable)
        {
            GenerateJsonConverter(context, sb);   
        }

        if (context.Namespace is not null)
        {
            sb.Append("}");
        }

        return sb.ToString();
    }

    private static void GenerateEqualityOverrides(
        in ResultContext context,
        StringBuilder sb,
        string nameWithGenericTypeParameters)
    {
        sb.AppendLine($$"""
                                public static bool operator !=({{nameWithGenericTypeParameters}} left, {{nameWithGenericTypeParameters}} right)
                                {
                                    return !(left == right);
                                }
                                
                                public static bool operator ==({{nameWithGenericTypeParameters}} left, {{nameWithGenericTypeParameters}} right)
                                {
                                    return left.Equals(right);
                                }
                        
                                public override bool Equals(object? obj)
                                {
                                    return obj is {{nameWithGenericTypeParameters}} && Equals(({{nameWithGenericTypeParameters}})obj);
                                }

                        """);

        sb.AppendLine(context.ValueType.HasValue
            ? $$"""
                        public bool Equals({{nameWithGenericTypeParameters}}{{(context.TypeSymbol is TypeSymbol.Class ? "?" : string.Empty)}} other)
                        {
                            {{(context.TypeSymbol is TypeSymbol.Class ? "if (other is null) return false;" : string.Empty)}}
                        
                            if (IsSuccess)
                            {
                                return other.IsSuccess && EqualityComparer<TValue>.Default.Equals(_value, other._value);
                            }
                
                            return other.IsFailure && EqualityComparer<{{context.ErrorType.Name}}?>.Default.Equals(_error, other._error);
                        }
                
                        public override int GetHashCode()
                        {
                            return IsSuccess 
                                ? EqualityComparer<TValue>.Default.GetHashCode(_value)
                                : EqualityComparer<{{context.ErrorType.Name}}?>.Default.GetHashCode(_error);
                        }

                """
            : $$"""
                        public bool Equals({{nameWithGenericTypeParameters}} other)
                        {
                            if (IsSuccess)
                            {
                                return other.IsSuccess;
                            }
                
                            return other.IsFailure && EqualityComparer<{{context.ErrorType.Name}}?>.Default.Equals(_error, other._error);
                        }
                
                        public override int GetHashCode()
                        {
                            return IsSuccess
                                ? IsSuccess.GetHashCode()
                                : EqualityComparer<{{context.ErrorType.Name}}?>.Default.GetHashCode(_error);
                        }

                """);
    }

    private static void GenerateImplicitOperators(
        in ResultContext context,
        StringBuilder sb,
        string nameWithGenericTypeParameters)
    {
        if (context.ValueType.HasValue)
        {
            sb.AppendLine($$"""
                                    public static implicit operator {{nameWithGenericTypeParameters}}({{context.ValueType.Value.Name}} value)
                                    {
                                        return Ok(value);
                                    }

                            """);
        }

        if (context.ErrorType.IsInterface is false)
        {
            sb.AppendLine($$"""
                                    public static implicit operator {{nameWithGenericTypeParameters}}({{context.ErrorType.Name}} error)
                                    {
                                        return Fail(error);
                                    }

                            """);
        }
    }

    private static void GenerateToStringMethod(in ResultContext context, StringBuilder sb)
    {
        sb.Append(
            $$$"""
                       /// <summary>
                       /// Returns a string that represents <see cref="{{{context.Name}}}"/>.
                       /// </summary>
                       public override string ToString()
                       {
                           return
                               $"{{{context.Name}}} {{ IsSuccess = {IsSuccess}
               """);

        if (context.ValueType.HasValue)
        {
            sb.Append("""{(IsSuccess ? $", Value = {_value.ToString()}" : string.Empty)}""");
        }

        sb.AppendLine("""{(IsFailure ? $", Error = {_error.ToString()}" : string.Empty)} }}";""");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private static void GenerateJsonConverter(in ResultContext context, StringBuilder sb)
    {
        var nameWithEmptyGenerics = context.ErrorType.IsGeneric
            ? $"{context.Name}<,>"
            : context.ValueType.HasValue
                ? $"{context.Name}<>"
                : context.Name;
        
        var classNameWithGenerics = GetClassNameWithGenerics(context);

        if (context.ValueType.HasValue)
        {
            sb.AppendLine($$"""
                            
                                public sealed class {{classNameWithGenerics}}JsonConverterFactory : System.Text.Json.Serialization.JsonConverterFactory
                                {
                                    public override bool CanConvert(Type typeToConvert)
                                    {
                                        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof({{nameWithEmptyGenerics}});
                                    }
                                
                                    public override System.Text.Json.Serialization.JsonConverter CreateConverter(Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
                                    {
                                        var genericArgument = typeToConvert.GetGenericArguments()[0];
                            """);

            sb.AppendLine(context.ErrorType.IsGeneric
                ? $"""
                              var genericArgument1 = typeToConvert.GetGenericArguments()[1];
                              var converterType = typeof({classNameWithGenerics}JsonConverter<,>).MakeGenericType(genericArgument, genericArgument1);
                  """
                : $"            var converterType = typeof({classNameWithGenerics}JsonConverter<>).MakeGenericType(genericArgument);");

            sb.AppendLine("""
                                      return (System.Text.Json.Serialization.JsonConverter)Activator.CreateInstance(converterType)!;
                                  }
                              }
                          """);
        }

        var generics = context.ErrorType.IsGeneric
            ? "<TValue, TError>"
            : context.ValueType.HasValue
                ? "<TValue>"
                : string.Empty;

        sb.AppendLine($$"""
                        
                            public sealed class {{classNameWithGenerics}}JsonConverter{{generics}} : System.Text.Json.Serialization.JsonConverter<{{context.Name}}{{generics}}>
                            {
                                public override {{context.Name}}{{generics}} Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
                                {
                                    using var document = System.Text.Json.JsonDocument.ParseValue(ref reader);
                            
                                    var root = document.RootElement;
                            
                                    var isSuccess = root.GetProperty("IsSuccess").GetBoolean();
                            
                                    if (isSuccess)
                                    {
                        """);

        sb.AppendLine(context.ValueType.HasValue
            ? $"""
                               var value = System.Text.Json.JsonSerializer.Deserialize<TValue>(root.GetProperty("Value"));
                               return {context.Name}{generics}.Ok(value!);
               """
            : $"                return {context.Name}.Ok();");

        var errorName = context.ErrorType.IsGeneric ? "TError" : context.ErrorType.Name;

        sb.AppendLine($$"""
                                    }
                                    
                                    var error = System.Text.Json.JsonSerializer.Deserialize<{{errorName}}>(root.GetProperty("Error"));
                                    return {{context.Name}}{{generics}}.Fail(error!);
                                }
                                
                                public override void Write(System.Text.Json.Utf8JsonWriter writer, {{context.Name}}{{generics}} value, System.Text.Json.JsonSerializerOptions options)
                                {
                                    writer.WriteStartObject();
                                    
                                    writer.WriteBoolean(nameof(value.IsSuccess), value.IsSuccess);
                                
                        """);

        if (context.ValueType.HasValue)
        {
            sb.AppendLine("""
                                      if (value.IsSuccess)
                                      {
                                          writer.WritePropertyName(nameof(value.Value));
                                          System.Text.Json.JsonSerializer.Serialize(writer, value.Value, options);
                                      }
                                      
                          """);
        }

        sb.AppendLine("""
                                  if (value.IsFailure)
                                  {
                                      writer.WritePropertyName(nameof(value.Error));
                                      System.Text.Json.JsonSerializer.Serialize(writer, value.Error, options);
                                  }
                                  
                                  writer.WriteEndObject();
                              }
                          }
                      """);
    }

    private static string GetClassNameWithGenerics(in ResultContext context)
    {
        if (context.ValueType.HasValue is false)
        {
            return context.Name;
        }
        
        return context.ErrorType.IsGeneric
            ? $"{context.Name}OfTValueTError"
            : $"{context.Name}OfTValue";
    }
}