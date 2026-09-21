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
    public sealed class ProductReviewService : IProductReviewService
    {
        private readonly IProductRepository _productRepository; 
        private readonly IProductReviewRepository _productReviewRepository;
        private readonly IExternalProductClient _externalProductClient;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductReviewService> _logger;
        public ProductReviewService(
            IProductRepository productRepository,
            IExternalProductClient externalProductClient,
            IMapper mapper,
            ILogger<ProductReviewService> logger,
            IProductReviewRepository productReviewRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper; 
            _externalProductClient = externalProductClient;
            _logger = logger;
            _productReviewRepository = productReviewRepository;
        }
    
        public async Task<ResponseDto<ProductWithReviewsDto>> GetByIdProductsWithReviewsAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new ArgumentException("Product id must be a positive integer.");

            ProductWithReviewsDto? cached = await _productReviewRepository.GetWithProductsReviewsByIdAsync(id, cancellationToken);
            if (cached is not null)
                return ResponseDto<ProductWithReviewsDto>.Success(cached);

            ExternalProductDto? external = await _externalProductClient.GetByIdAsync(id, cancellationToken);
            if (external is null)
                throw new KeyNotFoundException("Product " + id + " was not found.");

            await _productReviewRepository.CreateProductWithReviewsAsync(external, cancellationToken);
            return ResponseDto<ProductWithReviewsDto>.Success(_mapper.Map<ProductWithReviewsDto>(external));
        }
    } 
}
