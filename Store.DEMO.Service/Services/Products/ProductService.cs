using AutoMapper;
using Store.DEMO.Core;
using Store.DEMO.Core.Dtos;
using Store.DEMO.Core.Entites;
using Store.DEMO.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Service.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Repository<Product, int>().GetAllAsync();
            var productsMapp = _mapper.Map<IEnumerable<ProductDto>>(products);
            return productsMapp;
        }

        public async Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync()
        {
            return _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync());
        }

        public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync()
        {
            return _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductType, int>().GetAllAsync());

        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product , int>().GetAsync(id);
            var productMapp = _mapper.Map<ProductDto>(product);
            return productMapp;
        }
    }
}
