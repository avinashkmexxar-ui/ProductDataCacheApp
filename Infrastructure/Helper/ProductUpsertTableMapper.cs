using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Helper
{
    public static class ProductUpsertTableMapper
    {
        public static DataTable ToDataTable(IReadOnlyList<Product> products)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("DiscountPercentage", typeof(decimal));
            table.Columns.Add("Rating", typeof(decimal));
            table.Columns.Add("Stock", typeof(int));
            table.Columns.Add("Brand", typeof(string));
            table.Columns.Add("Sku", typeof(string));
            table.Columns.Add("Weight", typeof(decimal));
            table.Columns.Add("WarrantyInformation", typeof(string));
            table.Columns.Add("ShippingInformation", typeof(string));
            table.Columns.Add("AvailabilityStatus", typeof(string));
            table.Columns.Add("ReturnPolicy", typeof(string));
            table.Columns.Add("MinimumOrderQuantity", typeof(int));

            foreach (var product in products)
            {
                table.Rows.Add(
                    product.Id,
                    product.Title,
                    product.Description,
                    product.Category,
                    product.Price,
                    product.DiscountPercentage,
                    product.Rating,
                    product.Stock,
                    product.Brand,
                    product.Sku,
                    product.Weight,
                    product.WarrantyInformation,
                    product.ShippingInformation,
                    product.AvailabilityStatus,
                    product.ReturnPolicy,
                    product.MinimumOrderQuantity);
            }

            return table;
        }
    }
}
