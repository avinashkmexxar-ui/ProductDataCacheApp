using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Infrastructure.Repositories
{
    public sealed class ExternalProductClient : IExternalProductClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public ExternalProductClient(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            using var response = await _httpClient
                .GetAsync("products?limit=1000", cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var payload = await response.Content
                .ReadFromJsonAsync<ExternalProductListDto>(cancellationToken);

            return _mapper.Map<List<Product>>(payload?.Products ?? []);
        }

        public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
