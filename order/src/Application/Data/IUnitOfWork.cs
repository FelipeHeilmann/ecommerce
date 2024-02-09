namespace Application.Data;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
