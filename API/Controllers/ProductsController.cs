using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using API.Dtos;

using Core.Entities;
using Core.Interfaces;
using Core.Specifications;

using Infrastructure.Data;
using API.Errors;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        // private readonly IProductRepository _repos;
        // public ProductsController(IProductRepository repos)
        // {
        //     _repos = repos;
        // }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<Product>> GetProduct(int id)
        // {
        //     return await _repos.GetProductByIdAsync(id);
        // }

        // [HttpGet]
        // public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        // {
        //     var product = await _repos.GetProductsAsync();
        //     return Ok(product);
        // }

        // [HttpGet("brands")]

        // public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(){
        //   return Ok(await _repos.GetProductBrandsAsync());
        // }

        // [HttpGet("types")]
        // public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductType(){
        //     return Ok(await _repos.GetProductTypesAsync());            
        // }
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productRepo,
                                  IGenericRepository<ProductBrand> productBrandRepo,
                                  IGenericRepository<ProductType> productTypeRepo,
                                  IMapper mapper)
        {
            _productRepo = productRepo;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDtos>>> GetProduct()
        {
            // return Ok(await _productRepo.ListAllAsync());
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productRepo.ListAsync(spec);
            // DTO
            // return products.Select(product => new ProductToReturnDtos
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     PictureUrl = product.PictureUrl,
            //     Price = product.Price,
            //     ProductType = product.ProductType.Name,
            //     ProductBrand = product.ProductBrand.Name
            // }).ToList();

            //Auto mapper
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDtos>>(products));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProductToReturnDtos>> GetProductById(int id)
        {
            //return await _productRepo.GetByIdAsync(id);
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productRepo.GetEntityWithSpec(spec);
            if(product == null) return NotFound(new ApiResponse(404));
            //DTO
            // return new ProductToReturnDtos
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     Price = product.Price,
            //     PictureUrl = product.PictureUrl,
            //     ProductType = product.ProductType.Name,
            //     ProductBrand = product.ProductBrand.Name
            // };
            return _mapper.Map<Product, ProductToReturnDtos>(product); // map from product to productToReturnDtos
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrand()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetProductType()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}