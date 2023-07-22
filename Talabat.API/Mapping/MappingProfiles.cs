using AutoMapper;
using Talabat.API.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using AddressIdentity = Talabat.Core.Entities.Identity.Address;
using Address = Talabat.Core.Entities.Order_Aggregate.Address;
namespace Talabat.API.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name));
            //.ForMember(P => P.ProductBrand, O => O.MapFrom(s => s.ProductBrand.Name))
            //.ForMember(P => P.ProductType, O => O.MapFrom(S => S.ProductType.Name));

            CreateMap<AddressIdentity, AddressDTO >().ReverseMap();

            CreateMap<BasketItemDTO, BasketItem>().ReverseMap();
            CreateMap<CustomerBasketDTO, CustomerBasket>().ReverseMap();
            CreateMap<AddressDTO,Address>().ReverseMap();

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.ProductItem.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.ProductItem.ProductName))
                .ForMember(d => d.PicUrl, O => O.MapFrom(S => S.ProductItem.PicUrl))
                .ReverseMap();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(s => s.DeliveryMethod.Cost))
                .ReverseMap();


        }
    }
}
