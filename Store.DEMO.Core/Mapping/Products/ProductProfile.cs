using AutoMapper;
using Store.DEMO.Core.Dtos;
using Store.DEMO.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ForMember(P => P.BrandName, option => option.MapFrom(S => S.Brand.Name))
                                            .ForMember(P => P.TypeName, option => option.MapFrom(S => S.Type.Name));

            CreateMap<ProductBrand, TypeBrandDto>();
            CreateMap<ProductType, TypeBrandDto>();
        }
    }
}
