using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase:ComponentBase
    {
        [Inject]// inject a dependency into the ProductService property
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }
        public string ErrorMessage { get; set; }
        protected override async Task OnInitializedAsync()//life cycle events on microsoft .net web page
        {
            try
            {
                await ClearLocalStorage();
                //unneccessary trip to the server will be avoided
                Products = await ManageProductsLocalStorageService.GetCollection();
                //show how many items are stored 
                var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection() ;
                
                var totalQty = shoppingCartItems.Sum(i => i.Qty);

                ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
                
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }
        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
                   group product by product.CategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        } 
        protected string GetCategoryName(IGrouping<int,ProductDto> groupedProductDtos) {
            return groupedProductDtos.FirstOrDefault(pg => pg.CategoryId == groupedProductDtos.Key).CategoryName;
        }
        private async Task ClearLocalStorage()
        {
            //remove the relevant datafrom local storage 
            await ManageProductsLocalStorageService.RemoveCollection();
            await ManageCartItemsLocalStorageService.RemoveCollection();

        }
    }
}
