using Domain.Common; 

namespace Domain.Entities
{
    public sealed class Product : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Rating { get; set; }
        public int Stock { get; set; }
        public string? Brand { get; set; }
        public string? Sku { get; set; }
        public decimal Weight { get; set; }
        public string? WarrantyInformation { get; set; }
        public string? ShippingInformation { get; set; }
        public string? AvailabilityStatus { get; set; }
        public string? ReturnPolicy { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public List<ProductReview>? Reviews { get; set; } = [];
    }
}
