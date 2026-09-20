using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository; 
        private readonly IExternalProductClient _externalProductClient;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        public ProductService(
            IProductRepository productRepository,
            IExternalProductClient externalProductClient,
            IMapper mapper,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper; 
            _externalProductClient = externalProductClient;
            _logger = logger;
        }
        public async Task<ResponseDto<ProductDetailDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetById started. ProductId={ProductId}", id);
            if (id <= 0)
                throw new ArgumentException("Product id must be a positive integer.");

            var cached = await _productRepository.GetByIdAsync(id, cancellationToken);
            if (cached is not null)
                return ResponseDto<ProductDetailDto>.Success(_mapper.Map<ProductDetailDto>(cached));

            var external = await _externalProductClient.GetByIdAsync(id, cancellationToken);
            if (external is null)
                throw new KeyNotFoundException("Product " + id + " was not found.");
           
            await _productRepository.UpsertAsync([external], cancellationToken); 
            return ResponseDto<ProductDetailDto>.Success(_mapper.Map<ProductDetailDto>(external));
        }

        public async Task<ResponseDto<IReadOnlyList<ProductDetailDto>>> GetListAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetList started. Checking for cached products"); 
            IReadOnlyList<Product> products;  
            var cachedCount = await _productRepository.GetCountAsync(cancellationToken);
            var externatCount = await _externalProductClient.GetTotalCountAsync(cancellationToken);
            if (cachedCount < externatCount)
            {
                _logger.LogInformation("No products found in cache. Fetching from external source.");
                IReadOnlyList<ExternalProductDto> external = await _externalProductClient.GetAllAsync(cancellationToken);
                await _productRepository.UpsertAsync(external, cancellationToken); 
            }
            products = await _productRepository.GetListAsync(cancellationToken);
            return ResponseDto<IReadOnlyList<ProductDetailDto>>.Success(_mapper.Map<IReadOnlyList<ProductDetailDto>>(products));
        } 
    } 
}
 