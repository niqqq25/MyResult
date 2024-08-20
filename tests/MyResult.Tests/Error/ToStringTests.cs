namespace MyResult.Tests.Error;

public sealed class ToStringTests
{
    [Fact]
    public void ToString_ErrorWithCodeAndMessage_ReturnsCorrectString()
    {
        // Arrange
        var error = new MyResult.Error("code", "description");

        // Act
        var str = error.ToString();

        // Assert
        Assert.Equal("Error { Code = code, Description = description }", str);
    }

    [Fact]
    public void ToString_ErrorWithCodeMessageAndMetadata_ReturnsCorrectString()
    {
        // Arrange
        var error = new MyResult.Error(
            "code",
            "description",
            metadata: new Dictionary<string, object> { { "x", 1 }, { "y", 2 } });

        // Act
        var str = error.ToString();

        // Assert
        Assert.Equal("Error { Code = code, Description = description, Metadata = {x = 1,y = 2} }", str);
    }

    [Fact]
    public void ToString_ErrorWithCodeMessageMetadataAndInnerError_ReturnsCorrectString()
    {
        // Arrange
        var error = new MyResult.Error(
            "code",
            "description",
            innerErrors: [ new MyResult.Error("innerCode", "innerDescription") ],
            metadata: new Dictionary<string, object> { { "x", 1 }, { "y", 2 } });

        // Act
        var str = error.ToString();

        // Assert
        Assert.Equal(
            "Error { Code = code, Description = description, Metadata = {x = 1,y = 2}, InnerErrors = [Error { Code = innerCode, Description = innerDescription }] }",
            str);
    }
}