using Domain.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface IExternalProductClient
    {
        Task<IReadOnlyList<ExternalProductDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<ExternalProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}
