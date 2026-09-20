--create database if does not exist
IF DB_ID(N'ProductCache') IS NULL
BEGIN
    CREATE DATABASE ProductCache;
END
GO

USE ProductCache;
GO

--create product table if does not exist

IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL
    DROP TABLE dbo.Products;
GO


CREATE TABLE dbo.Products
(
    Id                     INT             NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
    Title                  NVARCHAR(300)   NOT NULL,
    Description            NVARCHAR(MAX)   NULL,
    Category               NVARCHAR(100)   NULL,
    Price                  DECIMAL(18, 2)  NOT NULL CONSTRAINT DF_Products_Price DEFAULT (0),
    DiscountPercentage     DECIMAL(9, 4)   NOT NULL CONSTRAINT DF_Products_DiscountPercentage DEFAULT (0),
    Rating                 DECIMAL(9, 4)   NOT NULL CONSTRAINT DF_Products_Rating DEFAULT (0),
    Stock                  INT             NOT NULL CONSTRAINT DF_Products_Stock DEFAULT (0),
    Brand                  NVARCHAR(100)   NULL,
    Sku                    NVARCHAR(50)    NULL,
    Weight                 DECIMAL(18, 4)  NOT NULL CONSTRAINT DF_Products_Weight DEFAULT (0),
    WarrantyInformation    NVARCHAR(200)   NULL,
    ShippingInformation    NVARCHAR(200)   NULL,
    AvailabilityStatus     NVARCHAR(50)    NULL,
    ReturnPolicy           NVARCHAR(200)   NULL,
    MinimumOrderQuantity   INT             NOT NULL CONSTRAINT DF_Products_MinimumOrderQuantity DEFAULT (1)
);
GO

--- create Table Valued Parameter type for upsert operation

-- IF TYPE_ID(N'dbo.ProductUpsertType') IS NOT NULL
--    DROP TYPE dbo.ProductUpsertType;
--GO
DROP PROCEDURE IF EXISTS dbo.usp_UpsertProducts;
GO

DROP TYPE IF EXISTS dbo.ProductUpsertType;
GO

CREATE TYPE dbo.ProductUpsertType AS TABLE
(
    Id                   INT            NOT NULL PRIMARY KEY,
    Title                NVARCHAR(300)  NOT NULL,
    Description          NVARCHAR(MAX)  NULL,
    Category             NVARCHAR(100)  NULL,
    Price                DECIMAL(18, 2) NOT NULL,
    DiscountPercentage   DECIMAL(9, 4)  NOT NULL,
    Rating               DECIMAL(9, 4)  NOT NULL,
    Stock                INT            NOT NULL,
    Brand                NVARCHAR(100)  NULL,
    Sku                  NVARCHAR(50)   NULL,
    Weight               DECIMAL(18, 4) NOT NULL,
    WarrantyInformation  NVARCHAR(200)  NULL,
    ShippingInformation  NVARCHAR(200)  NULL,
    AvailabilityStatus   NVARCHAR(50)   NULL,
    ReturnPolicy         NVARCHAR(200)  NULL,
    MinimumOrderQuantity INT            NOT NULL
);
GO

-- create stored procedure for upsert operation


CREATE PROCEDURE dbo.usp_UpsertProducts
    @Products dbo.ProductUpsertType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        MERGE dbo.Products AS target
        USING @Products AS source
        ON target.Id = source.Id
        WHEN MATCHED THEN
            UPDATE SET
                Title = source.Title,
                Description = source.Description,
                Category = source.Category,
                Price = source.Price,
                DiscountPercentage = source.DiscountPercentage,
                Rating = source.Rating,
                Stock = source.Stock,
                Brand = source.Brand,
                Sku = source.Sku,
                Weight = source.Weight,
                WarrantyInformation = source.WarrantyInformation,
                ShippingInformation = source.ShippingInformation,
                AvailabilityStatus = source.AvailabilityStatus,
                ReturnPolicy = source.ReturnPolicy,
                MinimumOrderQuantity = source.MinimumOrderQuantity
        WHEN NOT MATCHED THEN
            INSERT (
                Id, Title, Description, Category, Price, DiscountPercentage, Rating, Stock,
                Brand, Sku, Weight, WarrantyInformation, ShippingInformation, AvailabilityStatus,
                ReturnPolicy, MinimumOrderQuantity)
            VALUES (
                source.Id, source.Title, source.Description, source.Category, source.Price,
                source.DiscountPercentage, source.Rating, source.Stock, source.Brand, source.Sku,
                source.Weight, source.WarrantyInformation, source.ShippingInformation,
                source.AvailabilityStatus, source.ReturnPolicy, source.MinimumOrderQuantity);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END
GO


--create table to store Product Reviews 

IF TYPE_ID(N'Dbo.ProductReviews') IS NOT NULL
    DROP TYPE dbo.ProductReviews;
GO

CREATE TABLE dbo.ProductReviews
(
    Id             INT IDENTITY(1,1) NOT NULL CONSTRAINT  PK_ProductReviews PRIMARY KEY,
    ProductId      INT               NOT NULL CONSTRAINT FK_Reviews_Products FOREIGN KEY REFERENCES dbo.Products (Id),
    Rating         DECIMAL(9, 4)     NOT NULL,
    Comment        NVARCHAR(1000)    NULL,
    ReviewDate     DATETIME2         NULL,
    ReviewerName   NVARCHAR(200)     NULL,
    ReviewerEmail  NVARCHAR(256)     NULL
)
GO


---SELECT p.Id, p.Title, p.Brand, p.Category, p.Price,
 --                  p.Rating, r.Id AS ReviewId, r.Rating AS ReviewRating, r.Comment,
 --                  r.ReviewDate, r.ReviewerName, r.ReviewerEmail
 --           FROM Products AS p
 --           INNER JOIN ProductReviews AS r ON r.ProductId = p.Id
 --           WHERE p.Id = 5
 --           ORDER BY r.ReviewDate DESC, r.


-- USE ProductCache;
--GO

-- truncate table dbo.ProductReviews
-- truncate table Products

-- DELETE FROM ProductReviews;

--DELETE FROM Products;