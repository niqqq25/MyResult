#pragma warning disable 8766 // nullability of reference types in return type doesn't match implicitly implemented member

#nullable enable

namespace MyResult
{
    /// <summary>
    /// Represents the result of an operation, which can either be a success or a failure.
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(ResultJsonConverter))]
    public readonly record struct Result : IEquatable<Result>, IResult
    {
        private readonly Error? _error;

        /// <summary>
        /// Gets the error associated with the failed result.
        /// </summary>
        public Error? Error
        {
            get
            {
                if (IsSuccess)
                {
                    throw new System.InvalidOperationException($"{nameof(Result)} is success. {nameof(Error)} is not set.");
                }

                return _error;
            }
        }

        /// <summary>
        /// Checks whether the result represents a failed operation.
        /// </summary>
        /// <returns><c>true</c> if the operation failed; otherwise, <c>false</c>.</returns>
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(_error))]
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Error))]
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Checks whether the result represents a successful operation.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(_error))]
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(Error))]
        public bool IsSuccess { get; private init; }

        /// <summary>Creates a successful result.</summary>
        /// <returns>A new instance of <see cref="Result"/> representing a successful operation.</returns>
        public static Result Ok()
        {
            return new Result(default(Error))
            {
                IsSuccess = true
            };
        }

        /// <summary>Creates a failed result with the given error.</summary>
        /// <param name="error">The error associated with the failed result.</param>
        /// <returns>A new instance of <see cref="Result"/> representing a failed operation.</returns>
        public static Result Fail(Error error)
        {
            return new Result(error)
            {
                IsSuccess = false
            };
        }

        public static implicit operator Result(Error error)
        {
            return Fail(error);
        }

        [System.Obsolete("Must not be used without parameters", true)]
        public Result()
        {
        }

        private Result(Error? error)
        {
            _error = error;
        }

        /// <summary>
        /// Returns a string that represents <see cref="Result"/>.
        /// </summary>
        public override string ToString()
        {
            return
                $"Result {{ IsSuccess = {IsSuccess}{(IsFailure ? $", Error = {_error.ToString()}" : string.Empty)} }}";
        }

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
    }

    public sealed class ResultJsonConverter : System.Text.Json.Serialization.JsonConverter<Result>
    {
        public override Result Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            using var document = System.Text.Json.JsonDocument.ParseValue(ref reader);
    
            var root = document.RootElement;
    
            var isSuccess = root.GetProperty("IsSuccess").GetBoolean();
    
            if (isSuccess)
            {
                return Result.Ok();
            }
            
            var error = System.Text.Json.JsonSerializer.Deserialize<Error>(root.GetProperty("Error"));
            return Result.Fail(error!);
        }
        
        public override void Write(System.Text.Json.Utf8JsonWriter writer, Result value, System.Text.Json.JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            
            writer.WriteBoolean(nameof(value.IsSuccess), value.IsSuccess);
        
            if (value.IsFailure)
            {
                writer.WritePropertyName(nameof(value.Error));
                System.Text.Json.JsonSerializer.Serialize(writer, value.Error, value.Error.GetType(), options);
            }
            
            writer.WriteEndObject();
        }
    }
}