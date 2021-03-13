# gose
:beer: **Go** **s**tyle **e**rror handling for C#

## Rationale
`gose` is an experiment. Like the [beer of the same name](https://en.wikipedia.org/wiki/Gose), it may sound really gross.

[Go](https://golang.org/) has no concept of "throwing" exceptions and "catching" them. 
If an error occurs, it's either handled immediately, or returned by the function where the error occurred.
If a function can return a value or an error, a tuple like construct is used where the left hand side is the possible
successful result, and the right hand side is the possible error. If the result is an error, the left (success) side is nil.
If the result is a success, the right (error) side is nil.

For example:

```go
func canErr() (int, err) {
    result, err := someCallThatCanError()
    if err != nil {
        return (nil, err)
    }
    return (result, nil)
}
```