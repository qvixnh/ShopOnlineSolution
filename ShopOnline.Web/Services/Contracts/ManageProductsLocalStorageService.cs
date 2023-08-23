using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public class ManageProductsLocalStorageService : IManageProductsLocalStorageService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly IProductService productService;
        private const string key = "ProductCollection";
        // identify the data that is being stored in local storage
        // allows you to store multiple pieces of data in local storage without having to worry about them being overwritten.
        //the key's used to identify the relevant value - serialized collection of objects of type product
        public ManageProductsLocalStorageService(ILocalStorageService localStorageService,
                                                IProductService productService)
        {
            this.localStorageService = localStorageService;
            this.productService = productService;
        }
        //add collection
        private async Task<IEnumerable<ProductDto>> AddCollection()
        {
            //retrieves products data from server
            //saves the relevant product data in user's browser
            var productCollection = await this.productService.GetItems();
            if (productCollection != null)
            {
                await this.localStorageService.SetItemAsync(key, productCollection);
            }
            return productCollection;
        }
        public async Task<IEnumerable<ProductDto>> GetCollection()
        {
            //The ?? operator will return the result of the first operand if the first operand is not null.
            //If the first operand is null, the ?? operator will return the result of the second operand.
            return await this.localStorageService.GetItemAsync<IEnumerable<ProductDto>>(key) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await this.localStorageService.RemoveItemAsync(key);
        }
    }
}
