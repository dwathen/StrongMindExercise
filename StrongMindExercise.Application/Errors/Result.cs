namespace StrongMindExercise.Application.Errors;
public class Result<T> : Result
{
    private Result(bool isSuccess, Error error, T? data = default) : base(isSuccess, error)
    {
        this.Data = data;
    }

    public T? Data { get; }

    public static Result<T> Success(T data) => new(true, Error.None, data);
    public static new Result<T> Failure(Error error) => new(false, error);
}

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        this.IsSuccess = isSuccess;
        this.Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);
}