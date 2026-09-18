using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.SQL
{
    internal static class Queries
    {
        public const string ProductTable = "Products";
        public const string ProductUpsertProcedure = "dbo.usp_UpsertProducts";
        public const string ProductUpsertType = "dbo.ProductUpsertType";
        public const string GetProductList = $"SELECT * FROM {ProductTable}";
        public const string GetProductById = $"SELECT * FROM {ProductTable} WHERE Id = @Id";
    }
}
