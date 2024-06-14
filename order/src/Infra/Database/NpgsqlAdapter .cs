
using System.ComponentModel;
using System.Data;
using Domain.Customers.Entity;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infra.Database;

public class NpgsqlAdapter : IDatabaseConnection
{
    private readonly NpgsqlConnection _connection;

    public NpgsqlAdapter(IConfiguration configuration)
    {
        _connection = new NpgsqlConnection(configuration.GetValue<string>("ConnectionStrings:Database") ?? throw new ArgumentException());
    }
    public async Task<IEnumerable<T>> Query<T>(string statement, object? parameters = null, Func<IDataReader, T>? map = null)
    {
        await _connection.OpenAsync();

            using var cmd = new NpgsqlCommand(statement, _connection);

            if (parameters != null)
            {
                foreach (var property in parameters.GetType().GetProperties())
                {
                    cmd.Parameters.AddWithValue(property.Name, property.GetValue(parameters) ?? throw new Exception());
                }
            }

            var results = new List<T>();
            var reader = await cmd.ExecuteReaderAsync();

             while (await reader.ReadAsync())
            {
                if(map is not null) 
                {
                    var result = map(reader);
                    results.Add(result);
                }
                else 
                {
                    var result = await reader.GetFieldValueAsync<T>(0);
                    results.Add(result);
                }         
            }
            await _connection.CloseAsync();
            return results;     
    }

    public async Task Close()
    {
        await _connection.CloseAsync();
    }
}
