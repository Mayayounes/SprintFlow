namespace sprintFlow.Domain.Exceptions;

public class NotAuthorizedException : Exception
{
    public NotAuthorizedException(string resourceType = "User", string action = "perform this action")
        : base($"{resourceType} is not authorized to {action}.")
    {
    }
}