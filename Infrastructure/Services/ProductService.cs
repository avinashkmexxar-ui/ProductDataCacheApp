using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Repositories;
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
        public ProductService(
            IProductRepository productRepository,
            IExternalProductClient externalProductClient,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper; 
            _externalProductClient = externalProductClient;
        }
        public async Task<ResponseDto<ProductDetailDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new ArgumentException("Product id must be a positive integer.");

            var cached = await _productRepository.GetByIdAsync(id, cancellationToken);
            if (cached is not null)
            {
                return new ResponseDto<ProductDetailDto>
                {
                    IsSuccess = true,
                    Code = 200,
                    Message = "Successfully retrieved",
                    Data = _mapper.Map<ProductDetailDto>(cached)
                };
            }
            var external = await _externalProductClient.GetByIdAsync(id, cancellationToken);
            if (external is null)
                throw new KeyNotFoundException("Product " + id + " was not found.");
           
            await _productRepository.UpsertAsync([external], cancellationToken);
            return new ResponseDto<ProductDetailDto>
            {
                IsSuccess = true,
                Code = 200,
                Message = "Successfully retrieved",
                Data = _mapper.Map<ProductDetailDto>(external)
            }; 
        }

        public async Task<ResponseDto<IReadOnlyList<ProductDetailDto>>> GetListAsync(CancellationToken cancellationToken)
        {
            IReadOnlyList<Product> products = await _productRepository.GetListAsync(cancellationToken);
            if (products.Count == 0)
            {
                IReadOnlyList<Product> external = await _externalProductClient.GetAllAsync(cancellationToken);
                await _productRepository.UpsertAsync(external, cancellationToken);
                products = await _productRepository.GetListAsync(cancellationToken);
            }
             
            return new ResponseDto<IReadOnlyList<ProductDetailDto>>()
            {
                IsSuccess = true,
                Message = "Successfully retrieved",
                Data = _mapper.Map<IReadOnlyList<ProductDetailDto>>(products)
            };
        }
    } 
}
