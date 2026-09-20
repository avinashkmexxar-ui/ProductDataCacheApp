using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.SQL;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System.Data;

namespace Infrastructure.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        public const string ConnectionStringName = "ProductCache";
        private readonly string _connectionString;
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(IConfiguration configuration,
            ILogger<ProductRepository> logger)
        {
            var connectionString = configuration.GetConnectionString(ConnectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string  is not available.");
            }
            _connectionString = connectionString;
            _logger = logger;
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
                _logger.LogWarning("Product not found for ID: {ProductId}", id);
                return null;
            }
            _logger.LogDebug("Loaded cached product {ProductId}", id);
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
            _logger.LogDebug("Loaded cached products count: {ProductCount}", products.Count);
            return products;
        }

        

        public async Task UpsertAsync(IReadOnlyList<ExternalProductDto> products, CancellationToken cancellationToken)
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
            _logger.LogInformation("Upserted {ProductCount} products into SQL Server", products.Count);
        }
 
    }
}
