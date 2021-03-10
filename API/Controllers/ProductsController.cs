using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
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
        public ProductsController(IGenericRepository<Product> productRepo,
                                  IGenericRepository<ProductBrand> productBrandRepo,
                                  IGenericRepository<ProductType> productTypeRepo)
        {
            _productRepo = productRepo;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;

        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
           // return Ok(await _productRepo.ListAllAsync());
           var spec = new ProductsWithTypesAndBrandsSpecification();
           return Ok(await _productRepo.ListAsync(spec));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            //return await _productRepo.GetByIdAsync(id);
           var spec = new ProductsWithTypesAndBrandsSpecification(id);
           return await _productRepo.GetEntityWithSpec(spec);

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