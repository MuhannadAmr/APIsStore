using Store.DEMO.Core.Dtos;
using Store.DEMO.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Services.Contract
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync();
        Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
    }
}
