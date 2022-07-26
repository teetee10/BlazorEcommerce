using BlazorEcommerce.Shared.DTOs;

namespace BlazorEcommerce.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
            .Where(p => p.Featured)
            .Include(p => p.Variants)
            .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            /*            var response = new ServiceResponse<List<Product>>
                        {
                            Data = await _context.Products.ToListAsync()
                        };*/
            /*            var products = await _context.Products.Include(p => p.Variants).ToListAsync();
                        var response = new ServiceResponse<List<Product>>
                        {
                            Data = products
                        }; return response; */

            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.Include(p => p.Variants).ToListAsync()
            };
            return response;

        }
        /*public async Task<ServiceResponse<Product>> GetProductById(int id)*/
        public async Task<ServiceResponse<Product>> GetProductsAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            var product = await _context.Products
                //For every Product we include a variants. Specifiying related entities to include in the query result
                .Include(p => p.Variants)
                //For every Variants we include a Productype Specifiying related entities to include in the query result
                .ThenInclude(v => v.ProductType)
                //Gets the Product with its a given ID
                .FirstOrDefaultAsync(p => p.Id == productId);
            /*var product = await _context.Products.FindAsync(productId);*/

            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this product does not exist.";
            }
            else
            {
                response.Data = product;
            }

            return response;
        }


        //Gets Products by Categories 
        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
                .Include(p => p.Variants)
                .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText)
        {
            var products = await FindProductsBySearchText(searchText);

            List<string> result = new List<string>();

            foreach (var product in products)
            {
                if (product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Title);
                }

                if (product.Description != null)
                {
                    var punctuation = product.Description.Where(char.IsPunctuation).Distinct().ToArray();
                    var words = product.Description.Split().Select(w => w.Trim(punctuation));
                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        
        public async Task<ServiceResponse<ProductSearchResultDTO>> SearchProducts(string searchText, int page)
        {
            var pageResults = 2f;
            var pageCount = Math.Ceiling((await FindProductsBySearchText(searchText)).Count / pageResults);

            var products = await _context.Products
                                .Where(p => p.Title.ToLower().Contains(searchText.ToLower())
                                || p.Description.ToLower().Contains(searchText.ToLower()))
                                .Include(p => p.Variants)
                                .Skip((page - 1) * (int)pageResults)
                                .Take((int)pageResults)
                                .ToListAsync();

            var response = new ServiceResponse<ProductSearchResultDTO>
            {
                Data = new ProductSearchResultDTO
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;


        }

        private async Task<List<Product>> FindProductsBySearchText(string searchText)
        {
            return await _context.Products
                    .Where(p => p.Title.ToLower().Contains(searchText.ToLower())
                    || p.Description.ToLower().Contains(searchText.ToLower()))
                    .Include(p => p.Variants)
                    .ToListAsync();
        }

    }
}
