using System.Text.Json;
using MyResult.Tests.Error;

namespace MyResult.Tests.ResultOfTValueTError;

public sealed class JsonConverterTests
{
    [Fact]
    public void Serialize_ResultIsSuccess_ReturnsSerializedResult()
    {
        // Arrange
        const int value = 60;
        var result = Result<int, string>.Ok(value);
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        
        // Assert
        Assert.Equal($$"""{"IsSuccess":true,"Value":{{value}}}""", serializedResult);
    }
    
    [Fact]
    public void Serialize_ResultIsFailure_ReturnsSerializedResult()
    {
        // Arrange
        const string error = "Error";
        var serializedError = JsonSerializer.Serialize(error);
        var result = Result<int, string>.Fail(error);
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        
        // Assert
        Assert.Equal(
            $$"""{"IsSuccess":false,"Error":{{serializedError}}}""",
            serializedResult);
    }
    
    [Fact]
    public void Serialize_ResultWithDerivedError_ReturnsSerializedResultWithDerivedProperty()
    {
        // Arrange
        var error = new DerivedError("Code", "Description", "Extra");
        var serializedError = JsonSerializer.Serialize(error);
        Result<int, MyResult.Error> result = error;
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        
        // Assert
        Assert.Equal(
            $$"""{"IsSuccess":false,"Error":{{serializedError}}}""",
            serializedResult);
    }

    [Fact]
    public void Deserialize_ResultIsSuccess_ReturnsDeserializeResult()
    {
        // Arrange
        var result = Result<int, string>.Ok(50);
        var serializedResult = JsonSerializer.Serialize(result);
        
        // Act
        var deserializedResult = JsonSerializer.Deserialize<Result<int, string>>(serializedResult);
        
        // Assert
        Assert.True(deserializedResult.IsSuccess);
        Assert.Equal(deserializedResult.Value, result.Value);
    }

    [Fact]
    public void Deserialize_ResultIsFailure_ReturnsDeserializeResult()
    {
        // Arrange
        var result = Result<int, string>.Fail("Error");
        var serializedResult = JsonSerializer.Serialize(result);
        
        // Act
        var deserializedResult = JsonSerializer.Deserialize<Result<int, string>>(serializedResult);

        // Assert
        Assert.False(deserializedResult.IsSuccess);
        Assert.Equal(result.Error, deserializedResult.Error);
    }
}