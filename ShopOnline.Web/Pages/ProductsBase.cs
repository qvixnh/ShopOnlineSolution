using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase:ComponentBase
    {
        [Inject]// inject a dependency into the ProductService property
        public IProductService ProductService { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
        protected override async Task OnInitializedAsync()//life cycle events on microsoft .net web page
        {
            Products = await ProductService.GetItems();
        }
    }
}
