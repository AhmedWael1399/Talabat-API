using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatApi.Controllers;
using TalabatApi.Errors;
using TalabatCore.DTOs;
using TalabatCore.Entities;
using TalabatCore.Repositories;
using TalabatCore.Specifications;
using TalabatServices.Helpers;

namespace Talabat.APIS.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;

        public ProductsController(IGenericRepository<Product> productRepo, IMapper mapper, IGenericRepository<ProductType> typeRepo, IGenericRepository<ProductBrand> brandRepo)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _typeRepo = typeRepo;
            _brandRepo = brandRepo;
        }

        #region Get All Products

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductsSpecificationParams Params)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(Params);
            var Products = await _productRepo.GetAllWithSpecificationAsync(Spec);
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);

            var ProductToReturnDto = new Pagination<ProductToReturnDto>()
            {
                PageIndex = Params.PageIndex,
                PageSize = Params.PageSize,
                Data = MappedProducts
            };

            //return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex, Params.PageSize, MappedProducts));
            return Ok(ProductToReturnDto);
        }

        #endregion

        #region Get Product by Id

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id);
            var Product = await _productRepo.GetByIdWithSpecification(Spec);
            if (Product is null) return NotFound(new ApiResponse(404));
            var MappedProduct = _mapper.Map<Product, ProductToReturnDto>(Product);
            return Ok(MappedProduct);
        }

        #endregion

        #region Get Types

        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetTypes()
        {
            var Types = await _typeRepo.GetAllAsync();
            return Ok(Types);
        }

        #endregion

        #region Get Brands

        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetBrands()
        {
            var Brands = await _brandRepo.GetAllAsync();
            return Ok(Brands);
        }

        #endregion
    }
}
