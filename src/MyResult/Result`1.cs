#pragma warning disable 8766 // nullability of reference types in return type doesn't match implicitly implemented member

#nullable enable

namespace MyResult
{
    /// <summary>
    /// Represents the result of an operation, which can either be a success or a failure.
    /// </summary>
    /// <typeparam name="TValue">The type of the value associated with the successful result.</typeparam>
    [System.Text.Json.Serialization.JsonConverter(typeof(ResultOfTValueJsonConverterFactory))]
    public readonly record struct Result<TValue> : IEquatable<Result<TValue>>, IResult<TValue>
    {
        private readonly Error? _error;

        private readonly TValue? _value;

        /// <summary>
        /// Gets the error associated with the failed result.
        /// </summary>
        public Error? Error
        {
            get
            {
                if (IsSuccess)
                {
                    throw new System.InvalidOperationException($"{nameof(Result<TValue>)} is success. {nameof(Error)} is not set.");
                }

                return _error;
            }
        }

        /// <summary>
        /// Gets the value associated with the successful result.
        /// </summary>
        public TValue Value
        {
            get
            {
                if (IsFailure)
                {
                    throw new System.InvalidOperationException($"{nameof(Result<TValue>)} is failure. {nameof(Value)} is not set.");
                }

                return _value;
            }
        }

        /// <summary>
        /// Checks whether the result represents a failed operation.
        /// </summary>
        /// <returns><c>true</c> if the operation failed; otherwise, <c>false</c>.</returns>
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(_value))]
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(Value))]
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(_error))]
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Error))]
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Checks whether the result represents a successful operation.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(_value))]
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Value))]
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(_error))]
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(Error))]
        public bool IsSuccess { get; private init; }

        /// <summary>Creates a successful result with the given value.</summary>
        /// <param name="value">The value associated with the successful result.</param>
        /// <returns>A new instance of <see cref="Result"/> representing a successful operation.</returns>
        public static Result<TValue> Ok(TValue value)
        {
            return new Result<TValue>(value, default(Error))
            {
                IsSuccess = true
            };
        }

        /// <summary>Creates a failed result with the given error.</summary>
        /// <param name="error">The error associated with the failed result.</param>
        /// <returns>A new instance of <see cref="Result"/> representing a failed operation.</returns>
        public static Result<TValue> Fail(Error error)
        {
            return new Result<TValue>(default, error)
            {
                IsSuccess = false
            };
        }

        public static implicit operator Result<TValue>(TValue value)
        {
            return Ok(value);
        }

        public static implicit operator Result<TValue>(Error error)
        {
            return Fail(error);
        }

        [System.Obsolete("Must not be used without parameters", true)]
        public Result()
        {
        }

        private Result(TValue? value, Error? error)
        {
            _error = error;
            _value = value;
        }

        /// <summary>
        /// Returns a string that represents <see cref="Result"/>.
        /// </summary>
        public override string ToString()
        {
            return
                $"Result {{ IsSuccess = {IsSuccess}{(IsSuccess ? $", Value = {_value.ToString()}" : string.Empty)}{(IsFailure ? $", Error = {_error.ToString()}" : string.Empty)} }}";
        }

    }

    public sealed class ResultOfTValueJsonConverterFactory : System.Text.Json.Serialization.JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Result<>);
        }
    
        public override System.Text.Json.Serialization.JsonConverter CreateConverter(Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            var genericArgument = typeToConvert.GetGenericArguments()[0];
            var converterType = typeof(ResultOfTValueJsonConverter<>).MakeGenericType(genericArgument);
            return (System.Text.Json.Serialization.JsonConverter)Activator.CreateInstance(converterType)!;
        }
    }

    public sealed class ResultOfTValueJsonConverter<TValue> : System.Text.Json.Serialization.JsonConverter<Result<TValue>>
    {
        public override Result<TValue> Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            using var document = System.Text.Json.JsonDocument.ParseValue(ref reader);
    
            var root = document.RootElement;
    
            var isSuccess = root.GetProperty("IsSuccess").GetBoolean();
    
            if (isSuccess)
            {
                var value = System.Text.Json.JsonSerializer.Deserialize<TValue>(root.GetProperty("Value"));
                return Result<TValue>.Ok(value!);
            }
            
            var error = System.Text.Json.JsonSerializer.Deserialize<Error>(root.GetProperty("Error"));
            return Result<TValue>.Fail(error!);
        }
        
        public override void Write(System.Text.Json.Utf8JsonWriter writer, Result<TValue> value, System.Text.Json.JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            
            writer.WriteBoolean(nameof(value.IsSuccess), value.IsSuccess);
        
            if (value.IsSuccess)
            {
                writer.WritePropertyName(nameof(value.Value));
                System.Text.Json.JsonSerializer.Serialize(writer, value.Value, value.Value.GetType(), options);
            }
            
            if (value.IsFailure)
            {
                writer.WritePropertyName(nameof(value.Error));
                System.Text.Json.JsonSerializer.Serialize(writer, value.Error, value.Error.GetType(), options);
            }
            
            writer.WriteEndObject();
        }
    }
}