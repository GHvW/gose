# gose
:beer: **GO** **S**tyle **E**rror handling for C#

## Rationale
`gose` is an experiment. Like the [beer of the same name](https://en.wikipedia.org/wiki/Gose), it may sound really gross. But it wouldn't exist if no one liked it, right?

I'm exploring using Go style error handling in C#. If you believe in reserving exceptions for the "truly exceptional" you need a something other than exceptions to handle errors in C#. C# doesn't have an form of Union type so a true `Either` or `Result` type isn't an option at this point. You could easily make your own error handling type, but then you either need to recreate it in every project, or create a library around it and use that library in every project. If you build other libraries with your new error type and expose that type in the library's API, everyone who uses your other libraries is forced into your error handling package as well.

I'm wondering if Go style error handling can give us a convention for error handling that's built into the language. If we build libraries with the pattern, we're not exposing anything our consumers don't already have. Tuples are built into C#. 

Power users can extend the tuple convention with their own sets of methods. Developers who just want to get data and get on with it can use tranditional null checking and move on.


#### Go Error Handling for the Uninitiated

[Go](https://golang.org/) has no concept of "throwing" exceptions and "catching" them. 
If an error occurs, it's either handled immediately, or returned by the function where the error occurred.

If a function can return a value or an error, a tuple like construct is used where the left hand side is the possible
successful result, and the right hand side is the possible error. If the result is an error, the left (success) side is nil and the right (error) side contains data. If the result is a success, the right (error) side is nil and the left (success) side contains data.

For example:

```go
// hopefully this is valid go ><
func canErr() (data int, err error) {
    result, err := someFunctionThatCanError()
    if err != nil {
        return (nil, err)
    }
    return (result, nil)
}
```

No `try/catch` no `throw`. In some ways it reminds me of a less robust `Either` or `Result` type.

Go is able to enforce the convention with a great suite of tools. Could we do something similar in C#?

## The Convention
I don't write a lot of Go, or really any. My experience with it is primarily for study. Because of that I can never remember which
side the error goes on in the tuple. Sometimes it's the same for `Result` in F#. Haskell's `Either`, when used for errors, has "Success" on the right and the "Error" on the left. It's easy for me to remember because "right is right" since "right" is somewhat analogous to "success". 
I'm hoping it's as easy to remember for others, so in the tuple, the convention I use is the left side is the error side, and the right is the success side.
```chsarp
(error, success)
```
Unfortunately this differs from Go's convention. Windows' idea to switch up the file path slash to `\` instead of `/` was fine right? :laughing:

## Where gose fits

This library is an exploration of a set of those power user extensions on top of this convetion.
I also want to investigate if a rosalyn analyzer could provide compile time and/or in editor checks to make sure the convention is used correctly.

## Examples
#### Error in first result retains error through the expression
```csharp
(string? Error, int? Data) first = ("no data available", null); 
(string? Error, int? Data) second = (null, 20);

var result = 
    (from x in first
     from y in second
     select x + y);

Assert.Equal("no data available", result.Error);
Assert.Null(result.Data);
```

#### Error in second result
```csharp
(string? Error, int? Data) first = (null, 10); 
(string? Error, int? Data) second = ("divide by zero", null);

var result = 
    (from x in first
     from y in second
     select x + y);

Assert.Equal("divide by zero", result.Error);
Assert.Null(result.Data);
```

#### Success in both results allows calculation
```csharp
(string? Error, int? Data) first = (null, 10); 
(string? Error, int? Data) second = (null, 20);

var result = 
    (from x in first
     from y in second
     select x + y);

Assert.Equal(30, result.Data);
Assert.Null(result.Error);
```