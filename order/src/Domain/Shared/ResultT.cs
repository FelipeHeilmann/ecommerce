namespace Domain.Shared;

public class Result<T> : Result
{
    public T? Data { get; }

    public Result(T data) : base(null)
    {
        Data = data;
    }

    public Result(Error error) : base(error: error)
    {
    }

    public static implicit operator T(Result<T> result) =>
        result.IsFailure
        ? throw new InvalidOperationException() : result.Data!;

    public static implicit operator Result<T>(T result) =>
        Result.Success(result);

    public static implicit operator Result<T>(Error error) =>
        Result.Failure<T>(error);
}