namespace sprintFlow.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static Result<T> Success(T data, string? message = null)
        => new Result<T> { IsSuccess = true, Data = data, Message = message };

    public static Result<T> Failure(List<string> errors, string? message = null)
        => new Result<T> { IsSuccess = false, Errors = errors, Message = message };
}