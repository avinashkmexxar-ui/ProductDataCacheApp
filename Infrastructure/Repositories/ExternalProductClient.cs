using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
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
        public async Task<IReadOnlyList<ExternalProductDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            using var response = await _httpClient
                .GetAsync("products?limit=1000", cancellationToken);

            response.EnsureSuccessStatusCode();

            var payload = await response.Content
                .ReadFromJsonAsync<ExternalProductListDto>(cancellationToken);

            return _mapper.Map<List<ExternalProductDto>>(payload?.Products ?? []);
        }

        public async Task<ExternalProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            using var response = await _httpClient
               .GetAsync($"products/{id}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var payload = await response.Content
                .ReadFromJsonAsync<ExternalProductDto>(cancellationToken);

            return _mapper.Map<ExternalProductDto>(payload);
        }
        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
        {
            using var response = await _httpClient
                .GetAsync("products?limit=1", cancellationToken);

            response.EnsureSuccessStatusCode();

            var payload = await response.Content
                .ReadFromJsonAsync<ExternalProductListDto>(cancellationToken);

            return payload?.Total ?? 0;
        }
    }
}
