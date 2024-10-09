using Store.DEMO.Core.Dtos;
using Store.DEMO.Core.Entites;
using Store.DEMO.Core.Helper;
using Store.DEMO.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Services.Contract
{
    public interface IProductService
    {
        Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams param);
        Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync();
        Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
    }
}
