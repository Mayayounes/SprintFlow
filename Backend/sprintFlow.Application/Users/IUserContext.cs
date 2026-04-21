namespace sprintFlow.Application.Users;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}
