
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public sealed  class ExternalProductReviewDto
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? Date { get; set; }
        public string? ReviewerName { get; set; }
        public string? ReviewerEmail { get; set; }
    }
}
