using System.Text.Json;

namespace MyResult.Tests.Result;

public sealed class JsonConverterTests
{
    [Fact]
    public void Serialize_ResultIsSuccess_ReturnsSerializedResult()
    {
        // Arrange
        var result = MyResult.Result.Ok();
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        
        // Assert
        Assert.Equal("""{"IsSuccess":true}""", serializedResult);
    }
    
    [Fact]
    public void Serialize_ResultIsFailure_ReturnsSerializedResult()
    {
        // Arrange
        var error = new MyResult.Error("Code", "Description");
        var serializedError = JsonSerializer.Serialize(error);
        MyResult.Result result = error;
        
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
        var result = MyResult.Result.Ok();
        var serializedResult = JsonSerializer.Serialize(result);
        
        // Act
        var deserializedResult = JsonSerializer.Deserialize<MyResult.Result>(serializedResult);
        
        // Assert
        Assert.True(deserializedResult.IsSuccess);
    }

    [Fact]
    public void Deserialize_ResultIsFailure_ReturnsDeserializeResult()
    {
        // Arrange
        MyResult.Result result = new MyResult.Error("Code", "Description");
        var serializedResult = JsonSerializer.Serialize(result);
        
        // Act
        var deserializedResult = JsonSerializer.Deserialize<MyResult.Result>(serializedResult);

        // Assert
        Assert.False(deserializedResult.IsSuccess);
        Assert.Equal(result.Error!.Code, deserializedResult.Error.Code);
        Assert.Equal(result.Error!.Description, deserializedResult.Error.Description);
    }
}