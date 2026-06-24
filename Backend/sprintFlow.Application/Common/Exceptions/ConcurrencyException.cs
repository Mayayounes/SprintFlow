namespace sprintFlow.Application.Common.Exceptions;
public class ConcurrencyException : Exception
{
    public ConcurrencyException()
        : base("This record was modified by another user , Try again")
    {
    }
}