namespace Domain.Shared;

public class Result
{
    public Error? Error { get; private set; }

    protected Result(Error? error)
    {
        Error = error;
    }

    internal Result()
    {
    }

    public static Result Success() => new();

    public static Result Failure(Error error) => new(error);

    public static Result<T> Success<T>(T data) => new(data);

    public static Result<T> Failure<T>(Error error) => new(error);

    public bool IsSuccess => Error is null;

    public bool IsFailure => Error is not null;

    public bool HasError<T>() where T : Error => Error is T;

    public static implicit operator Error(Result result) =>
        !result.IsFailure
        ? throw new InvalidOperationException() : result.Error!;


    public static implicit operator Result(Error error) =>
        Result.Failure(error);
}

