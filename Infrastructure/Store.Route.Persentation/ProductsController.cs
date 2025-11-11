using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Route.Persentation.Attributes;
using Store.Route.Services.Abstractions;
using Store.Route.Shared;
using Store.Route.Shared.Dtos;
using Store.Route.Shared.ErrorModel;
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
        [ProducesResponseType(typeof(PaginationResponse<ProductResponse>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status500InternalServerError)]
        [Cache(60)]
        
        public async Task<ActionResult<PaginationResponse<ProductResponse>>> GetAllProducts([FromQuery]ProductQueryParameters parameters)
        {
            var Result=await _serviceManager.ProductService.GetAllProductsAsync(parameters);
            if (Result is null) return BadRequest();//404
             return Ok(Result);//200
        }

        [HttpGet("{id}")]//GET:baseUrl/api/Products/id
        [ProducesResponseType(typeof(ProductResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponse>> GetProductById(int? id)
        {
            if (id is null) return BadRequest();//400
            var Result = await _serviceManager.ProductService.GetProductByIdAsync(id.Value);
            //if (Result is null) return NotFound();//404
            return Ok(Result);
        }

        [HttpGet("brands")]//GET:baseUrl/api/Products/brands

        [ProducesResponseType(typeof(IEnumerable<BrandTypeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<BrandTypeResponse>>> GetAllBrands()
        {
            var Result = await _serviceManager.ProductService.GetAllBrandsAsync();
            if (Result is null) return BadRequest();//400
            return Ok(Result);
        }


        [HttpGet("types")]//GET:BaseUrl/Products/types

        [ProducesResponseType(typeof(IEnumerable<BrandTypeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BrandTypeResponse>>> GetAllTypes()
        {
            var Result = await _serviceManager.ProductService.GetAllTypesAsync();
            if(Result is null) return BadRequest();
            return Ok(Result);

        }
    }
}
