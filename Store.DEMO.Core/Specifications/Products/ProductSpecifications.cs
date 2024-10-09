using Store.DEMO.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Specifications.Products
{
    public class ProductSpecifications : BaseSpecifications<Product, int>
    {
        public ProductSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }
        public ProductSpecifications(ProductSpecParams param):base
            (
                P =>  (string.IsNullOrEmpty(param.Serach) || P.Name.ToLower().Contains(param.Serach))
                      &&
                      (!param.brandId.HasValue || P.BrandId == param.brandId)
                      && 
                      (!param.typeId.HasValue ||P.TypeId == param.typeId)
            )
        {
            if (!string.IsNullOrEmpty(param.sort))
            {
                switch (param.sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }

            ApplyIncludes();

            ApplyPagination(param.pageSize * (param.pageIndex - 1), param.pageSize);
        }
        private void ApplyIncludes()
        {
            InCludes.Add(P => P.Brand);
            InCludes.Add(P => P.Type);
        }
    }
}
