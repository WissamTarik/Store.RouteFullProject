using Microsoft.AspNetCore.Mvc;
using Store.Route.Services.Abstractions;
using Store.Route.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Persentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
      
        [HttpGet]//GET:baseUrl/api/Products
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductQueryParameters parameters)
        {
            var Result=await _serviceManager.ProductService.GetAllProductsAsync(parameters);
            if (Result is null) return BadRequest();//404
             return Ok(Result);//200
        }

        [HttpGet("{id}")]//GET:baseUrl/api/Products/id
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest();//400
            var Result = await _serviceManager.ProductService.GetProductByIdAsync(id.Value);
            //if (Result is null) return NotFound();//404
            return Ok(Result);
        }

        [HttpGet("brands")]//GET:baseUrl/api/Products/brands
        public async Task<IActionResult> GetAllBrands()
        {
            var Result = await _serviceManager.ProductService.GetAllBrandsAsync();
            if (Result is null) return BadRequest();//400
            return Ok(Result);
        }


        [HttpGet("types")]//GET:BaseUrl/Products/types
        public async Task<IActionResult> GetAllTypes()
        {
            var Result = await _serviceManager.ProductService.GetAllTypesAsync();
            if(Result is null) return BadRequest();
            return Ok(Result);

        }
    }
}
