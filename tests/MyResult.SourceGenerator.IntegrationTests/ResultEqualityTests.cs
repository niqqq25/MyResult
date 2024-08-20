namespace MyResult.SourceGenerator.IntegrationTests;

public sealed class ResultEqualityTests
{
    [Fact]
    public void Equality_SuccessWithoutValue_Equal()
    {
        // Arrange
        var result1 = ClassResult.Ok();
        var result2 = ClassResult.Ok();

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.Equal(result1HashCode, result2HashCode);
        Assert.Equal(result1, result2);
        Assert.StrictEqual(result1, result2);
        Assert.True(result1 == result2);
        Assert.True(result1.Equals((object)result2));
        Assert.True(result1.Equals(result2));
        Assert.False(result1 != result2);
    }

    [Fact]
    public void Equality_SuccessWithSameStructValues_Equal()
    {
        // Arrange
        var result1 = ClassResultOfTValue<int>.Ok(10);
        var result2 = ClassResultOfTValue<int>.Ok(10);

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.Equal(result1HashCode, result2HashCode);
        Assert.Equal(result1, result2);
        Assert.StrictEqual(result1, result2);
        Assert.True(result1 == result2);
        Assert.True(result1.Equals((object)result2));
        Assert.True(result1.Equals(result2));
        Assert.False(result1 != result2);
    }

    [Fact]
    public void Equality_SuccessWithSameReferenceValues_Equal()
    {
        // Arrange
        var result1 = ClassResultOfTValue<RecordValue>.Ok(new RecordValue("10"));
        var result2 = ClassResultOfTValue<RecordValue>.Ok(new RecordValue("10"));

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.Equal(result1HashCode, result2HashCode);
        Assert.Equal(result1, result2);
        Assert.StrictEqual(result1, result2);
        Assert.True(result1 == result2);
        Assert.True(result1.Equals((object)result2));
        Assert.True(result1.Equals(result2));
        Assert.False(result1 != result2);
    }

    [Fact]
    public void Equality_SuccessWithDifferentStructValues_NotEqual()
    {
        // Arrange
        var result1 = ClassResultOfTValue<int>.Ok(60);
        var result2 = ClassResultOfTValue<int>.Ok(20);

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.NotEqual(result1HashCode, result2HashCode);
        Assert.NotEqual(result1, result2);
        Assert.NotStrictEqual(result1, result2);
        Assert.False(result1 == result2);
        Assert.False(result1.Equals((object)result2));
        Assert.False(result1.Equals(result2));
        Assert.True(result1 != result2);
    }

    [Fact]
    public void Equality_SuccessWithDifferentReferenceValues_NotEqual()
    {
        // Arrange
        var result1 = ClassResultOfTValue<RecordValue>.Ok(new RecordValue("10"));
        var result2 = ClassResultOfTValue<RecordValue>.Ok(new RecordValue("11"));

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.NotEqual(result1HashCode, result2HashCode);
        Assert.NotEqual(result1, result2);
        Assert.NotStrictEqual(result1, result2);
        Assert.False(result1 == result2);
        Assert.False(result1.Equals((object)result2));
        Assert.False(result1.Equals(result2));
        Assert.True(result1 != result2);
    }

    [Fact]
    public void Equality_FailWithSameEmbeddedReferenceErrors_Equal()
    {
        // Arrange
        var result1 = ClassResult.Fail(new Error("Oops"));
        var result2 = ClassResult.Fail(new Error("Oops"));

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.Equal(result1HashCode, result2HashCode);
        Assert.Equal(result1, result2);
        Assert.StrictEqual(result1, result2);
        Assert.True(result1 == result2);
        Assert.True(result1.Equals((object)result2));
        Assert.True(result1.Equals(result2));
        Assert.False(result1 != result2);
    }

    [Fact]
    public void Equality_FailWithSameEmbeddedStructErrors_Equal()
    {
        // Arrange
        var result1 = ResultWithStructError.Fail(new StructError("Oops"));
        var result2 = ResultWithStructError.Fail(new StructError("Oops"));

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.Equal(result1HashCode, result2HashCode);
        Assert.Equal(result1, result2);
        Assert.StrictEqual(result1, result2);
        Assert.True(result1 == result2);
        Assert.True(result1.Equals((object)result2));
        Assert.True(result1.Equals(result2));
        Assert.False(result1 != result2);
    }

    [Fact]
    public void Equality_FailWithDifferentEmbeddedReferenceErrors_NotEqual()
    {
        // Arrange
        var result1 = ClassResult.Fail(new Error("Oops"));
        var result2 = ClassResult.Fail(new Error("Whoops"));

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.NotEqual(result1HashCode, result2HashCode);
        Assert.NotEqual(result1, result2);
        Assert.NotStrictEqual(result1, result2);
        Assert.False(result1 == result2);
        Assert.False(result1.Equals((object)result2));
        Assert.False(result1.Equals(result2));
        Assert.True(result1 != result2);
    }

    [Fact]
    public void Equality_FailWithDifferentEmbeddedStructErrors_NotEqual()
    {
        // Arrange
        var result1 = ResultWithStructError.Fail(new StructError("Oops"));
        var result2 = ResultWithStructError.Fail(new StructError("Whoops"));

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.NotEqual(result1HashCode, result2HashCode);
        Assert.NotEqual(result1, result2);
        Assert.NotStrictEqual(result1, result2);
        Assert.False(result1 == result2);
        Assert.False(result1.Equals((object)result2));
        Assert.False(result1.Equals(result2));
        Assert.True(result1 != result2);
    }

    [Fact]
    public void Equality_FailWithSameReferenceErrors_Equal()
    {
        // Arrange
        var result1 = ClassResultOfTValueTError<int, Error>.Fail(new Error("Oops"));
        var result2 = ClassResultOfTValueTError<int, Error>.Fail(new Error("Oops"));

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.Equal(result1HashCode, result2HashCode);
        Assert.Equal(result1, result2);
        Assert.StrictEqual(result1, result2);
        Assert.True(result1 == result2);
        Assert.True(result1.Equals((object)result2));
        Assert.True(result1.Equals(result2));
        Assert.False(result1 != result2);
    }

    [Fact]
    public void Equality_FailWithSameStructErrors_Equal()
    {
        // Arrange
        var result1 = ClassResultOfTValueTError<int, StructError>.Fail(new StructError("Oops"));
        var result2 = ClassResultOfTValueTError<int, StructError>.Fail(new StructError("Oops"));

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.Equal(result1HashCode, result2HashCode);
        Assert.Equal(result1, result2);
        Assert.StrictEqual(result1, result2);
        Assert.True(result1 == result2);
        Assert.True(result1.Equals((object)result2));
        Assert.True(result1.Equals(result2));
        Assert.False(result1 != result2);
    }

    [Fact]
    public void Equality_FailWithDifferentReferenceErrors_NotEqual()
    {
        // Arrange
        var result1 = ClassResultOfTValueTError<int, Error>.Fail(new Error("Oops"));
        var result2 = ClassResultOfTValueTError<int, Error>.Fail(new Error("Whoops"));

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.NotEqual(result1HashCode, result2HashCode);
        Assert.NotEqual(result1, result2);
        Assert.NotStrictEqual(result1, result2);
        Assert.False(result1 == result2);
        Assert.False(result1.Equals((object)result2));
        Assert.False(result1.Equals(result2));
        Assert.True(result1 != result2);
    }

    [Fact]
    public void Equality_FailWithDifferentStructErrors_NotEqual()
    {
        // Arrange
        var result1 = ClassResultOfTValueTError<int, StructError>.Fail(new StructError("Oops"));
        var result2 = ClassResultOfTValueTError<int, StructError>.Fail(new StructError("Whoops"));

        // Act
        var result1HashCode = result1.GetHashCode();
        var result2HashCode = result2.GetHashCode();

        // Assert
        Assert.NotEqual(result1HashCode, result2HashCode);
        Assert.NotEqual(result1, result2);
        Assert.NotStrictEqual(result1, result2);
        Assert.False(result1 == result2);
        Assert.False(result1.Equals((object)result2));
        Assert.False(result1.Equals(result2));
        Assert.True(result1 != result2);
    }
}

public record RecordValue(string Name);