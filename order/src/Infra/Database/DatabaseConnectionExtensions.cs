
namespace Infra.Database;
public static class DatabaseConnectionExtensions
{
    public static async Task<T> QuerySingle<T>(this IDatabaseConnection connection, string statement, object? parameters = null)
        {
            var results = await connection.Query<T>(statement, parameters);
            return results.Single(); 
        } 
}
