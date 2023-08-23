using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly IShoppingCartService shoppingCartService;
        private const string key = "CartItemCollection";
        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService,  IShoppingCartService shoppingCartService)
        {
            this.localStorageService = localStorageService;
            this.shoppingCartService = shoppingCartService;
        }
        private async Task<List<CartItemDto>> AddCollecion()
        {
            var shoppingCartCollection =  await this.shoppingCartService.GetItems(HardCoded.UserId);
            if(shoppingCartCollection != null)
            {
                await this.localStorageService.SetItemAsync(key, shoppingCartCollection);
            }
            return shoppingCartCollection;
        }
        public async Task<List<CartItemDto>> GetCollection()
        {
            return await this.localStorageService.GetItemAsync<List<CartItemDto>>(key) ?? await AddCollecion();
        }

        public async Task RemoveCollection()
        {
            await this.localStorageService.RemoveItemAsync(key);
        }

        public async Task SaveCollection(List<CartItemDto> cartItemDtos)
        {
            await this.localStorageService.SetItemAsync(key, cartItemDtos);
        }
    }
}
