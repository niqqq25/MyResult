<div align="center">

<h1>MyResult</h1>

[![NuGet](https://img.shields.io/nuget/v/MyResult)](https://www.nuget.org/packages/MyResult) [![GitHub](https://img.shields.io/github/license/niqqq25/MyResult)](LICENSE)

A modern .NET library offering a simple implementation of the Result pattern and allowing you to build flexible custom results suited to your needs.

</div>
  
- [Getting started](#getting-started)
  - [Creating a successful result](#%EF%B8%8F-creating-a-successful-result)
    - [Using The `Ok` static method](#using-the-ok-static-method)
    - [Using implicit conversion](#using-implicit-conversion)
  - [Creating a failed result](#-creating-a-failed-result)
    - [Using The `Fail` static method](#using-the-fail-static-method)
    - [Using implicit conversion](#using-implicit-conversion-1)
  - [Checking the state of a result](#-checking-the-state-of-a-result)
    - [`IsSuccess`](#issuccess)
    - [`IsFailure`](#isfailure)
  - [Getting the contents of a result](#-checking-the-state-of-a-result)
    - [`Value`](#value)
    - [`Error`](#error)
- [Source Generation](#source-generation)

# Getting Started

MyResult has three different types of the result:
- `Result` - an operation result without a value.
- `Result<TValue>` - an operation result with a value.
- `Result<TValue, TError>` - an operation result with a value and a custom error.

## ✔️ Creating a Successful Result

### Using the `Ok` static method.

```csharp
// Result
var result = Result.Ok();

// Result<TValue>
var result = Result.Ok(50);

// Result<TValue, TError>
var result = Result.Ok<int, string>(50);
```

### Using implicit conversion.

```csharp
// Result<TValue>
Result<int> result = 50;

public Result<int> Foo()
{
    return 50;
}

// Result<TValue, TError>
Result<int, string> result = 50;

public Result<int, string> Foo()
{
    return 50;
}
```

## ❌ Creating a Failed Result

### Using the `Fail` static method.

```csharp
// Result
var result = Result.Fail(new Error("ErrorCode", "ErrorDescription"));

// Result<TValue>
var result = Result.Fail<int>(new Error("ErrorCode", "ErrorDescription"));

// Result<TValue, TError>
var result = Result.Fail<int, string>("Error");
```

### Using implicit conversion.

```csharp
// Result
var result = Result.Fail(new Error("ErrorCode", "ErrorDescription"));

public Result Foo()
{
    return new Error("ErrorCode", "ErrorDescription");
}

// Result<TValue>
Result<int> result = new Error("ErrorCode", "ErrorDescription");

public Result<int> Foo()
{
    return new Error("ErrorCode", "ErrorDescription");
}

// Result<TValue, TError>
Result<int, string> result = "Error";

public Result<int, string> Foo()
{
    return "Error";
}
```

## 🔎 Checking the State of the Result

### `IsSuccess`

```csharp
if (result.IsSuccess)
{
    // the result contains a value
}
```

### `IsFailure`

```csharp
if (result.IsFailure)
{
    // the result contains an error
}
```

## Getting the Contents of the Result

### `Value`

```csharp
if (result.IsSuccess)
{
    Console.WriteLine(result.Value);
}
```

### `Error`

```csharp
if (result.IsFailure)
{
    Console.WriteLine(result.Error);
}
```

# Source Generation

Coming soon

  







