using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.SQL
{
    internal static class Queries
    {
        public const string ProductTable = "Products";
        public const string ReviewTable = "ProductReviews";
        public const string ProductUpsertProcedure = "dbo.usp_UpsertProducts";
        public const string ProductUpsertType = "dbo.ProductUpsertType";
        public const string GetProductList = $"SELECT * FROM {ProductTable}";
        public const string GetProductById = $"SELECT * FROM {ProductTable} WHERE Id = @Id";

        public const string InsertProduct = $"""
            IF NOT EXISTS (SELECT Title FROM {ProductTable} WHERE Id = @Id)
            INSERT INTO {ProductTable} (
                Id, Title, Description, Category, Price, DiscountPercentage, Rating, Stock,
                Brand, Sku, Weight, WarrantyInformation, ShippingInformation, AvailabilityStatus,
                ReturnPolicy, MinimumOrderQuantity)
            VALUES (
                @Id, @Title, @Description, @Category, @Price, @DiscountPercentage, @Rating, @Stock,
                @Brand, @Sku, @Weight, @WarrantyInformation, @ShippingInformation, @AvailabilityStatus,
                @ReturnPolicy, @MinimumOrderQuantity)
            """;
        public const string UpsertProduct = $"""
            IF NOT EXISTS (SELECT 1 FROM {ProductTable} WHERE Id = @Id)
            BEGIN
                INSERT INTO {ProductTable} (
                    Id, Title, Description, Category, Price, DiscountPercentage, Rating, Stock,
                    Brand, Sku, Weight, WarrantyInformation, ShippingInformation, AvailabilityStatus,
                    ReturnPolicy, MinimumOrderQuantity)
                VALUES (
                    @Id, @Title, @Description, @Category, @Price, @DiscountPercentage, @Rating, @Stock,
                    @Brand, @Sku, @Weight, @WarrantyInformation, @ShippingInformation, @AvailabilityStatus,
                    @ReturnPolicy, @MinimumOrderQuantity);
            END
            ELSE
            BEGIN
                UPDATE {ProductTable}
                SET
                    Title = @Title,
                    Description = @Description,
                    Category = @Category,
                    Price = @Price,
                    DiscountPercentage = @DiscountPercentage,
                    Rating = @Rating,
                    Stock = @Stock,
                    Brand = @Brand,
                    Sku = @Sku,
                    Weight = @Weight,
                    WarrantyInformation = @WarrantyInformation,
                    ShippingInformation = @ShippingInformation,
                    AvailabilityStatus = @AvailabilityStatus,
                    ReturnPolicy = @ReturnPolicy,
                    MinimumOrderQuantity = @MinimumOrderQuantity
                WHERE Id = @Id;
            END
            """;

        public const string InsertReview = $"""
            INSERT INTO {ReviewTable} (ProductId, Rating, Comment, ReviewDate, ReviewerName, ReviewerEmail)
            VALUES (@ProductId, @Rating, @Comment, @ReviewDate, @ReviewerName, @ReviewerEmail)
            """;

        public const string GetProductWithReviewsById = $"""
            SELECT p.Id, p.Title, p.Brand, p.Category, p.Price,
                   p.Rating, r.Id AS ReviewId, r.Rating AS ReviewRating, r.Comment,
                   r.ReviewDate, r.ReviewerName, r.ReviewerEmail
            FROM {ProductTable} AS p
            INNER JOIN {ReviewTable} AS r ON r.ProductId = p.Id
            WHERE p.Id = @Id
            ORDER BY r.ReviewDate DESC, r.Id
            """;
    }
}
