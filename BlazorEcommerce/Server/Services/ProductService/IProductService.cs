namespace BlazorEcommerce.Server.Services.ProductService
{
    public interface IProductService
    {
        /*        Task<ServiceResponse<List<Product>>> GetProducts();
                Task<ServiceResponse<Product>> GetProductById(int id);
                Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl);
                Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int page);
                Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText);
                Task<ServiceResponse<List<Product>>> GetFeaturedProducts();*/

        Task<ServiceResponse<List<Product>>> GetProductsAsync();
        Task<ServiceResponse<Product>> GetProductsAsync(int productId);
        Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl);
        Task<ServiceResponse<List<Product>>> SearchProducts(string searchText);
        /*Task<ServiceResponse<Product>> SearchProducts(string searchText, int page);*/
        Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText);
    }
}
