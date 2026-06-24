namespace sprintFlow.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}