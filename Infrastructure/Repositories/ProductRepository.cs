using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.SQL;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        public const string ConnectionStringName = "ProductCache";
        private readonly string _connectionString;
        public ProductRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(ConnectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string  is not available.");
            }
            _connectionString = connectionString;
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = Queries.GetProductById;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                return null;
            }

            return ProductUpsertTableMapper.MapProduct(reader);
        }

        public async Task<IReadOnlyList<Product>> GetListAsync(CancellationToken cancellationToken)
        {
            List<Product> products = new();
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = Queries.GetProductList;

            await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                products.Add(ProductUpsertTableMapper.MapProduct(reader));
            }

            return products;
        }

        public async Task UpsertAsync(IReadOnlyList<Product> products, CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = Queries.ProductUpsertProcedure;

            var productsParameter = cmd.Parameters.Add("@Products", SqlDbType.Structured);
            productsParameter.TypeName = Queries.ProductUpsertType;
            productsParameter.Value = ProductUpsertTableMapper.ToDataTable(products);

            await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
            await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
