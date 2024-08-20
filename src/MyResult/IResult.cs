namespace MyResult;

public interface IResult<out TValue> : IResult
{
    /// <summary>
    /// Gets the value associated with the result.
    /// </summary>
    TValue Value { get; }
}

public interface IResult
{
    /// <summary>
    /// Gets the error associated with the result.
    /// </summary>
    Error Error { get; }

    /// <summary>
    /// Checks whether the result represents a successful operation.
    /// </summary>
    /// <returns><c>true</c> if the operation was successful; otherwise, false.</returns>
    bool IsSuccess { get; }

    /// <summary>
    /// Checks whether the result represents a failed operation.
    /// </summary>
    /// <returns><c>true</c> if the operation failed; otherwise, false.</returns>
    bool IsFailure { get; }
}