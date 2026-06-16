namespace sprintFlow.Application.Common.Exceptions;

public class ConcurrencyException : Exception
{
    public object? LatestState { get; }

    public ConcurrencyException(string message, object? latestState = null)
        : base(message)
    {
        LatestState = latestState;
    }
}