namespace Domain.Shared;

public class Result<T> : Result
{
    private T? data { get; }

    public Result(T Data) : base(null)
    {
        data = Data;
    }

    public Result(Error error) : base(error: error)
    {
    }

    public T GetValue() => data!;

    public static implicit operator T(Result<T> result) =>
        result.IsFailure
        ? throw new InvalidOperationException() : result.data!;

    public static implicit operator Result<T>(T result) =>
        Result.Success(result);

    public static implicit operator Result<T>(Error error) =>
        Result.Failure<T>(error);
}
