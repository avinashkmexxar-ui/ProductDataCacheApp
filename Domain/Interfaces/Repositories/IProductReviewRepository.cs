using Domain.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface IProductReviewRepository
    {
        Task<ProductWithReviewsDto?> GetWithProductsReviewsByIdAsync(int id, CancellationToken cancellationToken);
        Task CreateProductWithReviewsAsync(ExternalProductDto product, CancellationToken cancellationToken);
    }
}
