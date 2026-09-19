using Domain.Entities;
using Microsoft.Data.SqlClient;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.SQL
{
    public static class ProductUpsertTableMapper
    {
        public static DataTable ToDataTable(IReadOnlyList<ExternalProductDto> products)
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

        public static Product MapProduct(SqlDataReader reader)
        {
            return new Product
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
        }
        public static ProductWithReviewsDto MapSummariseProductWithProductReview(SqlDataReader reader)
        {
            return new ProductWithReviewsDto
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString(reader.GetOrdinal("Category")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                Rating = reader.GetDecimal(reader.GetOrdinal("Rating")),
                Brand = reader.IsDBNull(reader.GetOrdinal("Brand")) ? null : reader.GetString(reader.GetOrdinal("Brand")),
                Reviews =
                [
                    new ExternalProductReviewDto
                    { 
                        Rating = reader.GetInt32(reader.GetOrdinal("ReviewRating")),
                        Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? null : reader.GetString(reader.GetOrdinal("Comment")), 
                        ReviewerName = reader.IsDBNull(reader.GetOrdinal("ReviewerName")) ? null : reader.GetString(reader.GetOrdinal("ReviewerName")),
                        ReviewerEmail = reader.IsDBNull(reader.GetOrdinal("ReviewerEmail")) ? null : reader.GetString(reader.GetOrdinal("ReviewerEmail"))
                    }
                ]
            };
        }
    }
}
