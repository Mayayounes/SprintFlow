namespace sprintFlow.Application.Common.Exceptions;

public class ConcurrencyException : Exception
{
    public string EntityName { get; }
    public object? LatestState { get; }

    public ConcurrencyException(string entityName, string message, object? latestState = null)
        : base(message)
    {
        EntityName = entityName;
        LatestState = latestState;
    }
}
