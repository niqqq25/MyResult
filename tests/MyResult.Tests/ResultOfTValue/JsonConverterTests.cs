using System.Text.Json;
using MyResult.Tests.Error;

namespace MyResult.Tests.ResultOfTValue;

public sealed class JsonConverterTests
{
    [Fact]
    public void Serialize_ResultIsSuccess_ReturnsSerializedResult()
    {
        // Arrange
        const int value = 60;
        var result = MyResult.Result.Ok(value);

        // Act
        var serializedResult = JsonSerializer.Serialize(result);

        // Assert
        Assert.Equal($$"""{"IsSuccess":true,"Value":{{value}}}""", serializedResult);
    }

    [Fact]
    public void Serialize_ResultIsFailure_ReturnsSerializedResult()
    {
        // Arrange
        var error = new MyResult.Error("Code", "Description");
        var serializedError = JsonSerializer.Serialize(error);
        Result<int> result = error;

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
        Result<int> result = error;
        
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
        var result = MyResult.Result.Ok(50);
        var serializedResult = JsonSerializer.Serialize(result);

        // Act
        var deserializedResult = JsonSerializer.Deserialize<Result<int>>(serializedResult);

        // Assert
        Assert.True(deserializedResult.IsSuccess);
        Assert.Equal(deserializedResult.Value, result.Value);
    }

    [Fact]
    public void Deserialize_ResultIsFailure_ReturnsDeserializeResult()
    {
        // Arrange
        Result<int> result = new MyResult.Error("Code", "Description");
        var serializedResult = JsonSerializer.Serialize(result);

        // Act
        var deserializedResult = JsonSerializer.Deserialize<Result<int>>(serializedResult);

        // Assert
        Assert.False(deserializedResult.IsSuccess);
        Assert.Equal(result.Error!.Code, deserializedResult.Error.Code);
        Assert.Equal(result.Error!.Description, deserializedResult.Error.Description);
    }
}