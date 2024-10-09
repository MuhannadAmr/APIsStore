using AutoMapper;
using Store.DEMO.Core;
using Store.DEMO.Core.Dtos;
using Store.DEMO.Core.Entites;
using Store.DEMO.Core.Helper;
using Store.DEMO.Core.Services.Contract;
using Store.DEMO.Core.Specifications.Products;
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
        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams param)
        {
            var spec = new ProductSpecifications(param);

            var products = await _unitOfWork.Repository<Product, int>().GetAllWithSpecAsync(spec);
            var productsMapp = _mapper.Map<IEnumerable<ProductDto>>(products);

            var countSpec = new ProductWithCountSpec(param);
            var count = await _unitOfWork.Repository<Product, int>().GetCountAsync(countSpec);

            return new PaginationResponse<ProductDto>(param.pageSize, param.pageIndex, count, productsMapp);
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
            var spec = new ProductSpecifications(id);

            var product = await _unitOfWork.Repository<Product , int>().GetWithSpecAsync(spec);
            var productMapp = _mapper.Map<ProductDto>(product);
            return productMapp;
        }

    }
}
