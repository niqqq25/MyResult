using System.Text.Json;

namespace MyResult.SourceGenerator.IntegrationTests;

public sealed class ResultJsonSerializationTests
{
    [Fact]
    public void JsonSerialization_ResultWithoutErrorIsSuccess_SerializesAndDeserializes()
    {
        // Arrange
        var result = ClassResult.Ok();
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        var deserializedResult = JsonSerializer.Deserialize<ClassResult>(serializedResult)!;
        
        // Assert
        Assert.Equal("""{"IsSuccess":true}""", serializedResult);
        Assert.True(deserializedResult.IsSuccess);
    }
    
    [Fact]
    public void JsonSerialization_ResultWithoutErrorIsFailure_SerializesAndDeserializes()
    {
        // Arrange
        var error = new Error("Error");
        var serializedError = JsonSerializer.Serialize(error);
        var result = ClassResult.Fail(error);
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        var deserializedResult = JsonSerializer.Deserialize<ClassResult>(serializedResult)!;
        
        // Assert
        Assert.Equal($$"""{"IsSuccess":false,"Error":{{serializedError}}}""", serializedResult);
        Assert.False(deserializedResult.IsSuccess);
        Assert.Equal(error.Message, deserializedResult.Error.Message);
    }

    [Fact]
    public void JsonSerialization_ResultWithValueIsSuccess_SerializesAndDeserializes()
    {
        // Arrange
        const int value = 50;
        var result = ClassResultOfTValue<int>.Ok(value);
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        var deserializedResult = JsonSerializer.Deserialize<ClassResultOfTValue<int>>(serializedResult)!;
        
        // Assert
        Assert.Equal($$"""{"IsSuccess":true,"Value":{{value}}}""", serializedResult);
        Assert.True(deserializedResult.IsSuccess);
        Assert.Equal(value, deserializedResult.Value);
    }

    [Fact]
    public void JsonSerialization_ResultWithValueIsFailure_SerializesAndDeserializes()
    {
        // Arrange
        var error = new Error("Error");
        var serializedError = JsonSerializer.Serialize(error);
        var result = ClassResultOfTValue<int>.Fail(error);
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        var deserializedResult = JsonSerializer.Deserialize<ClassResultOfTValue<int>>(serializedResult)!;
        
        // Assert
        Assert.Equal($$"""{"IsSuccess":false,"Error":{{serializedError}}}""", serializedResult);
        Assert.False(deserializedResult.IsSuccess);
        Assert.Equal(error.Message, deserializedResult.Error.Message);
    }

    [Fact]
    public void JsonSerialization_ResultWithGenericErrorIsSuccess_SerializesAndDeserializes()
    {
        // Arrange
        const int value = 50;
        var result = ClassResultOfTValueTError<int, Error>.Ok(value);
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        var deserializedResult = JsonSerializer.Deserialize<ClassResultOfTValueTError<int, Error>>(serializedResult)!;
        
        // Assert
        Assert.Equal($$"""{"IsSuccess":true,"Value":{{value}}}""", serializedResult);
        Assert.True(deserializedResult.IsSuccess);
        Assert.Equal(value, deserializedResult.Value);
    }
    
    [Fact]
    public void JsonSerialization_ResultWithGenericErrorIsFailure_SerializesAndDeserializes()
    {
        // Arrange
        var error = new Error("Error");
        var serializedError = JsonSerializer.Serialize(error);
        var result = ClassResultOfTValueTError<int, Error>.Fail(error);
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        var deserializedResult = JsonSerializer.Deserialize<ClassResultOfTValueTError<int, Error>>(serializedResult)!;
        
        // Assert
        Assert.Equal($$"""{"IsSuccess":false,"Error":{{serializedError}}}""", serializedResult);
        Assert.False(deserializedResult.IsSuccess);
        Assert.Equal(error.Message, deserializedResult.Error.Message);
    }

    [Fact]
    public void JsonSerialization_ResultWithErrorInterfaceIsSuccess_SerializesAndDeserializes()
    {
        // Arrange
        var result = SerializableResultWithErrorInterface.Ok();
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        var deserializedResult = JsonSerializer.Deserialize<SerializableResultWithErrorInterface>(serializedResult)!;
        
        // Assert
        Assert.Equal("""{"IsSuccess":true}""", serializedResult);
        Assert.True(deserializedResult.IsSuccess);
    }

    [Fact]
    public void JsonSerialization_ResultWithErrorInterfaceIsFailure_SerializesAndThrowsOnDeserialization()
    {
        // Arrange
        var error = new SerializableResultError("Error");
        var serializedError = JsonSerializer.Serialize(error);
        var result = SerializableResultWithErrorInterface.Fail(error);

        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        var resultDeserialization = () =>
            JsonSerializer.Deserialize<SerializableResultWithErrorInterface>(serializedResult)!;
        
        // Assert
        Assert.Equal($$"""{"IsSuccess":false,"Error":{{serializedError}}}""", serializedResult);
        Assert.Throws<NotSupportedException>(resultDeserialization);
    }

    [Fact]
    public void JsonSerialization_ResultWithDerivedValue_SerializesDerivedPropertiesAndDeserializes()
    {
        // Arrange
        var value = new DerivedError("Error", "Extra");
        var serializedValue = JsonSerializer.Serialize(value);
        var result = ClassResultOfTValue<Error>.Ok(value);
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        var deserializedResult = JsonSerializer.Deserialize<ClassResultOfTValue<Error>>(serializedResult)!;
        
        // Assert
        Assert.Equal($$"""{"IsSuccess":true,"Value":{{serializedValue}}}""", serializedResult);
        Assert.True(deserializedResult.IsSuccess);
        Assert.Equal(value.Message, deserializedResult.Value.Message);
    }

    [Fact]
    public void JsonSerialization_ResultWithDerivedError_SerializesDerivedPropertiesAndDeserializes()
    {
        // Arrange
        var error = new DerivedError("Error", "Extra");
        var serializedError = JsonSerializer.Serialize(error);
        var result = ClassResult.Fail(error);
        
        // Act
        var serializedResult = JsonSerializer.Serialize(result);
        var deserializedResult = JsonSerializer.Deserialize<ClassResult>(serializedResult)!;
        
        // Assert
        Assert.Equal($$"""{"IsSuccess":false,"Error":{{serializedError}}}""", serializedResult);
        Assert.False(deserializedResult.IsSuccess);
        Assert.Equal(error.Message, deserializedResult.Error.Message);
    }
}

public interface ISerializableResultError
{
    public string Message { get; }
}

public record SerializableResultError(string Message) : ISerializableResultError;

[Result(typeof(ISerializableResultError), isSerializable: true)]
public partial record SerializableResultWithErrorInterface
{
}