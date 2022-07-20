using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class OrderMappings : Profile
{
    public OrderMappings()
    {
        CreateMap<OrderedProductCreateDto, OrderedProduct>()
            .ForMember(q => q.ProductTypeId, opt => opt.MapFrom(q => q.ProductId));

        CreateMap<Order, OrderDto>();

        CreateMap<OrderedProduct, OrderedProductDto>()
            .ForMember(q => q.ProductType, opt => opt.MapFrom(q => q.ProductType!.Name));
    }
}