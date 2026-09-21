using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public sealed class ProductWithReviewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public decimal Rating { get; set; }
        public List<ExternalProductReviewDto> Reviews { get; set; } = [];
    }
}
