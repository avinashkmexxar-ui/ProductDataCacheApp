using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public sealed class ProductReview
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string? ReviewerName { get; set; }
        public string? ReviewerEmail { get; set; }
    }
}
