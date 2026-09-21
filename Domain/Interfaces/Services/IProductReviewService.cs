using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services
{
    public interface IProductReviewService
    { 
        Task<ResponseDto<ProductWithReviewsDto>> GetByIdProductsWithReviewsAsync(int id,CancellationToken cancellationToken);

    }
}
