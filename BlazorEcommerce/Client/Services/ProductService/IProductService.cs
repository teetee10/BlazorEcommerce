﻿using BlazorEcommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorEcommerce.Client.Services.ProductService
{
    public interface IProductService
    {
        /*      event Action ProductsChanged;
              List<Product> Products { get; set; }
              string Message { get; set; }
              int CurrentPage { get; set; }
              int PageCount { get; set; }
              string LastSearchText { get; set; }
              Task SearchProducts(string searchText, int page);
              Task<List<string>> GetProductSearchSuggestions(string searchText);*/

        event Action ProductsChanged;
        List<Product> Products { get; set; }
        string Message { get; set; }
        int CurrentPage { get; set; }   
        int PageCount { get; set; } 
        string LastSearchText { get; set; } 
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProductById(int  productId);
        Task SearchProducts(string searchText, int page);
        Task<List<string>> GetProductSearchSuggestions(string searchText);

    }
}
