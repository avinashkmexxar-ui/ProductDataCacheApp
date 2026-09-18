using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public sealed class ExternalProductListDto
    {
        public List<ExternalProductDto> Products { get; set; } = [];
    }
}
