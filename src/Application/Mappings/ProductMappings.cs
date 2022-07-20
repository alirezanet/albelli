using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ProductMappings : Profile
{
    public ProductMappings()
    {
        CreateMap<ProductType, ProductDto>();
    }
}