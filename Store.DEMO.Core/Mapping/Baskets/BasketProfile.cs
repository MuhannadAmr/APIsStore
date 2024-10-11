using AutoMapper;
using Store.DEMO.Core.Dtos;
using Store.DEMO.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Mapping.Baskets
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustmerBasket, CustmerBasketDto>().ReverseMap();
        }
    }
}
