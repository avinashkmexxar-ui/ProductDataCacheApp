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
            throw new NotImplementedException();
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
                Message = "Successfully Created",
                Data = _mapper.Map<IReadOnlyList<ProductDetailDto>>(products)
            };
        }
    } 
}
