using System.Data;

namespace Infra.Database;

public interface IDatabaseConnection
{
    Task<IEnumerable<T>> Query<T>(string statement, object? parameters = null, Func<IDataReader, T>? map = null);
    Task Close();
}
