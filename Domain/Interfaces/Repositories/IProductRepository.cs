using Domain.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetListAsync(CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task UpsertAsync(IReadOnlyList<ExternalProductDto> products, CancellationToken cancellationToken);
        Task<int> GetCountAsync(CancellationToken cancellationToken);
    }
}
