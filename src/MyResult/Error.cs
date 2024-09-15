using System.Text.Json.Serialization;

namespace MyResult;

/// <summary>
/// Represents an error.
/// </summary>
public class Error
{
    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }
    
    public Error(string code, string description, IEnumerable<Error>? innerErrors)
    {
        Code = code;
        Description = description;
        InnerErrors = innerErrors;
    }
    
    public Error(string code, string description, IReadOnlyDictionary<string, object>? metadata)
    {
        Code = code;
        Description = description;
        Metadata = metadata;
    }
    
    [JsonConstructor]
    public Error(
        string code,
        string description,
        IEnumerable<Error>? innerErrors,
        IReadOnlyDictionary<string, object>? metadata)
    {
        Code = code;
        Description = description;
        InnerErrors = innerErrors;
        Metadata = metadata;
    }
    
    /// <summary>
    /// Gets the unique error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the collection of errors that caused the current error
    /// </summary>
    public IEnumerable<Error>? InnerErrors { get; }

    /// <summary>
    /// Gets the metadata associated with the error.
    /// </summary>
    public IReadOnlyDictionary<string, object>? Metadata { get; }

    public override string ToString()
    {
        var metadataAsString = Metadata is not null
            ? ", Metadata = {" + string.Join(",", Metadata.Select(d => $"{d.Key} = {d.Value}")) + "}"
            : null;
        var innerErrorsAsString = InnerErrors is not null
            ? ", InnerErrors = [" + string.Join(",", InnerErrors.Select(e => e.ToString())) + "]"
            : null;
        return $"Error {{ Code = {Code}, Description = {Description}{metadataAsString}{innerErrorsAsString} }}";
    }
}