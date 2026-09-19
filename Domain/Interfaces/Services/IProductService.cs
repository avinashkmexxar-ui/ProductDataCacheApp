using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<ResponseDto<IReadOnlyList<ProductDetailDto>>> GetListAsync(CancellationToken cancellationToken);

        Task<ResponseDto<ProductDetailDto>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<ResponseDto<ProductWithReviewsDto>> GetByIdProductsWithReviewsAsync(int id,CancellationToken cancellationToken);

    }
}
