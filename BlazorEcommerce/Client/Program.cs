global using BlazorEcommerce.Shared;
global using BlazorEcommerce.Shared.DTOs;
global using System.Net.Http.Json;
using BlazorEcommerce.Client;
using BlazorEcommerce.Client.Services.ProductService;
using BlazorEcommerce.Client.Services.CategoryService;
using BlazorEcommerce.Client.Services.CartService;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();

await builder.Build().RunAsync();
