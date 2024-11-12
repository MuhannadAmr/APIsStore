using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.DEMO.Core.Dtos.Auth;
using Store.DEMO.Core.Dtos.Orders;
using Store.DEMO.Core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Mapping.Orders
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile(IConfiguration configuration)
        {
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, option => option.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, option => option.MapFrom(s => s.DeliveryMethod.Cost))
                ;
            CreateMap<Address, AddressDto>()
                .ForMember(d=>d.FName,option => option.MapFrom(s=>s.FirstName))
                .ForMember(d=>d.LName,option => option.MapFrom(s=>s.LastName)).ReverseMap();
                
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, option => option.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, option => option.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PicureUrl, option => option.MapFrom(s => $"{configuration["BASEURL"]}{s.Product.PictureUrl}"));
        }
    }
}
