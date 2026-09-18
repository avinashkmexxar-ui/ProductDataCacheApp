using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Helper;
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
            cmd.CommandText = "SELECT * FROM Products WHERE Id = @Id";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                return null;
            }
            var product = new Product
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString(reader.GetOrdinal("Category")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                DiscountPercentage = reader.GetDecimal(reader.GetOrdinal("DiscountPercentage")),
                Rating = reader.GetDecimal(reader.GetOrdinal("Rating")),
                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                Brand = reader.IsDBNull(reader.GetOrdinal("Brand")) ? null : reader.GetString(reader.GetOrdinal("Brand")),
                Sku = reader.IsDBNull(reader.GetOrdinal("Sku")) ? null : reader.GetString(reader.GetOrdinal("Sku")),
                Weight = reader.GetDecimal(reader.GetOrdinal("Weight")),
                WarrantyInformation = reader.IsDBNull(reader.GetOrdinal("WarrantyInformation")) ? null : reader.GetString(reader.GetOrdinal("WarrantyInformation")),
                ShippingInformation = reader.IsDBNull(reader.GetOrdinal("ShippingInformation")) ? null : reader.GetString(reader.GetOrdinal("ShippingInformation")),
                AvailabilityStatus = reader.IsDBNull(reader.GetOrdinal("AvailabilityStatus")) ? null : reader.GetString(reader.GetOrdinal("AvailabilityStatus")),
                ReturnPolicy = reader.IsDBNull(reader.GetOrdinal("ReturnPolicy")) ? null : reader.GetString(reader.GetOrdinal("ReturnPolicy")),
                MinimumOrderQuantity = reader.GetInt32(reader.GetOrdinal("MinimumOrderQuantity"))
            };

            return product;
        }

        public async Task<IReadOnlyList<Product>> GetListAsync(CancellationToken cancellationToken)
        {
            List<Product> products = new();
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Products";

            await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                var product = new Product
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                    Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString(reader.GetOrdinal("Category")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    DiscountPercentage = reader.GetDecimal(reader.GetOrdinal("DiscountPercentage")),
                    Rating = reader.GetDecimal(reader.GetOrdinal("Rating")),
                    Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                    Brand = reader.IsDBNull(reader.GetOrdinal("Brand")) ? null : reader.GetString(reader.GetOrdinal("Brand")),
                    Sku = reader.IsDBNull(reader.GetOrdinal("Sku")) ? null : reader.GetString(reader.GetOrdinal("Sku")),
                    Weight = reader.GetDecimal(reader.GetOrdinal("Weight")),
                    WarrantyInformation = reader.IsDBNull(reader.GetOrdinal("WarrantyInformation")) ? null : reader.GetString(reader.GetOrdinal("WarrantyInformation")),
                    ShippingInformation = reader.IsDBNull(reader.GetOrdinal("ShippingInformation")) ? null : reader.GetString(reader.GetOrdinal("ShippingInformation")),
                    AvailabilityStatus = reader.IsDBNull(reader.GetOrdinal("AvailabilityStatus")) ? null : reader.GetString(reader.GetOrdinal("AvailabilityStatus")),
                    ReturnPolicy = reader.IsDBNull(reader.GetOrdinal("ReturnPolicy")) ? null : reader.GetString(reader.GetOrdinal("ReturnPolicy")),
                    MinimumOrderQuantity = reader.GetInt32(reader.GetOrdinal("MinimumOrderQuantity"))
                };
                products.Add(product);
            }

            return products;
        }

        public async Task UpsertAsync(IReadOnlyList<Product> products, CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.usp_UpsertProducts";

            var productsParameter = cmd.Parameters.Add("@Products", SqlDbType.Structured);
            productsParameter.TypeName = "dbo.ProductUpsertType";
            productsParameter.Value = ProductUpsertTableMapper.ToDataTable(products);

            await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
            await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
