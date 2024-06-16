using System.Data;
using Domain.Transactions.Entity;
using Domain.Transactions.Repository;
using Infra.Database;

namespace Infra.Repositories.Database;

public class TransactionRepositoryDatabase : ITransactionRepository
{
    private readonly IDatabaseConnection _connection;

    public TransactionRepositoryDatabase(IDatabaseConnection connection)
    {
        _connection = connection;
    }

    public async Task<ICollection<Transaction>> GetAllAsync(CancellationToken cancellationToken, string? include = null)
    {
        var transactions = await _connection.Query("select * from transactions", null, MapTransaction);
        return transactions.ToList();
    }

    public async Task<Transaction?> GetByGatewayServiceId(Guid id, CancellationToken cancellation)
    {
        var transactions = await _connection.Query("select * from transactions where payment_service_id = @Id", new { Id = id }, MapTransaction);
        return transactions.FirstOrDefault();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null)
    {
        var transactions = await _connection.Query("select * from transactions where id = @Id", new { Id = id }, MapTransaction);
        return transactions.FirstOrDefault();
    }

    public IQueryable<Transaction> GetQueryable(CancellationToken cancellation)
    {
        var transactions = _connection.Query("select * from transactions", null, MapTransaction).Result;
        return transactions.AsQueryable();
    }

    public async Task Add(Transaction entity)
    {
        await _connection.Query<Task>("insert into transactions (id, order_id, customer_id, payment_service_id, status, payment_type, created_at, approved_at, rejected_at) values (@Id, @OrderId, @CustomerId, @PaymentServiceId, @Status, @PaymentType, @CreatedAt, @ApprovedAt, @RejectedAt)", new {
            entity.Id,
            entity.OrderId,
            entity.CustomerId,
            entity.PaymentServiceId,
            entity.Status,
            entity.PaymentType,
            entity.CreatedAt,
            entity.ApprovedAt,
            entity.RejectedAt
        });
    }

    public async Task Update(Transaction entity)
    {
        await _connection.Query<Task>("update transactions set status = @Status, approved_at = @ApprovedAt, rejected_at = @RejectedAt where id = @Id", new {
            entity.Status,
            entity.ApprovedAt,
            entity.RejectedAt,
            entity.Id
        });
    }

    public async Task Delete(Transaction entity)
    {
        await _connection.Query<Task>("delete from transactions where id = @Id", new { entity.Id });
    }

    private Transaction MapTransaction(IDataReader reader)
    {
        return Transaction.Restore(
            reader.GetGuid(reader.GetOrdinal("id")),
            reader.GetGuid(reader.GetOrdinal("order_id")),
            reader.GetGuid(reader.GetOrdinal("customer_id")),
            reader.GetGuid(reader.GetOrdinal("payment_service_id")),
            reader.GetDouble(reader.GetOrdinal("amount")),
            reader.GetString(reader.GetOrdinal("payment_type")),
            reader.GetString(reader.GetOrdinal("status")),
            reader.GetDateTime(reader.GetOrdinal("created_at")),
            reader.IsDBNull(reader.GetOrdinal("approved_at")) ? null : reader.GetDateTime(reader.GetOrdinal("approved_at")),
            reader.IsDBNull(reader.GetOrdinal("rejected_at")) ? null : reader.GetDateTime(reader.GetOrdinal("rejected_at"))
        );
    }
    
}
