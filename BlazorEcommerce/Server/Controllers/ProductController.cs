using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            
            _productService = productService;
        }

        /*        [HttpGet]
                public async Task<ActionResult<List<Product>>> GetProducts()
                {
                    var products = await _context.Products.ToListAsync();
                    return Ok(products);
                }*/
        //BEST PRACTICES
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            var reult = await _productService.GetProductsAsync();
            return Ok(reult);
        }

        [HttpGet("{productId}")]
        /*public async Task<ActionResult<ServiceResponse<Product>>> GetProductById(int productId)*/
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId)
        {
            /*var result = await _productService.GetProductById(productId);*/
            var result = await _productService.GetProductsAsync(productId);
            return Ok(result);
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl)
        {
            var result = await _productService.GetProductsByCategory(categoryUrl);
            return Ok(result);
        }

        /*        [HttpGet("search/{searchText}/{page}")]
                public async Task<ActionResult<ServiceResponse<ProductSearchResult>>> SearchProducts(string searchText, int page = 1)*/
        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<ServiceResponse<Product>>> SearchProducts(string searchText)
        {
            /*var result = await _productService.SearchProducts(searchText, page);*/
            var result = await _productService.SearchProducts(searchText);
            return Ok(result);
        }

        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestions(string searchText)
        {
            var result = await _productService.GetProductSearchSuggestions(searchText);
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
        {
            var result = await _productService.GetFeaturedProducts();
            return Ok(result);
        }

    }
}
