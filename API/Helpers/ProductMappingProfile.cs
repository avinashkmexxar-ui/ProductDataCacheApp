using AutoMapper;
using Domain.Entities;
using Shared.DTOs;

namespace API.Helpers
{
    public sealed class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDetailDto>();
            CreateMap<ExternalProductDto, Product>();  
        }
    }
}
