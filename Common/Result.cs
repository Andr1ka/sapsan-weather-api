namespace Sapsan.WeatherApi.Common;

public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string Error { get; }
    public int StatusCode { get; }

    private Result(bool isSuccess, T? value, string error, int statusCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result<T> Ok(T value)
    {
        return new Result<T>(true, value, string.Empty, 200);
    }

    public static Result<T> Created(T value)
    {
        return new Result<T>(true, value, string.Empty, 201);
    }

    public static Result<T> Fail(string error, int statusCode)
    {
        return new Result<T>(false, default, error, statusCode);
    }
}
