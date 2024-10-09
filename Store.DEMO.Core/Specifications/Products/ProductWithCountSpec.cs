using Store.DEMO.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Specifications.Products
{
    public class ProductWithCountSpec : BaseSpecifications<Product, int>
    {
        public ProductWithCountSpec(ProductSpecParams param) : base
            (
                P => (string.IsNullOrEmpty(param.Serach) || P.Name.ToLower().Contains(param.Serach))
                      &&
                     (!param.brandId.HasValue || P.BrandId == param.brandId)
                     && 
                     (!param.typeId.HasValue || P.TypeId == param.typeId)
            )
        {
            
        }
    }
}
