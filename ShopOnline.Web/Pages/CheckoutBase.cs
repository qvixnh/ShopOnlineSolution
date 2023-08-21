using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class CheckoutBase:ComponentBase
    {
        ///Visa card
        //Card number 4032030061642576
        //Expiry date12/2025
        //CVC code355

        //Generated credit card details
        //Card number4137351560517315
        //Expiry date01/2024
        //CVC code497
        [Inject]
        public IJSRuntime Js { get; set; }

        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
        protected int TotalQty { get; set; }

        protected string PaymentDescription { get; set; }
        protected decimal PaymentAmount { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        protected override async void OnInitialized()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                if(ShoppingCartItems != null)
                {
                    Guid orderGuid = Guid.NewGuid();
                    PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                    TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                    PaymentDescription = $"O_{HardCoded.UserId}_{orderGuid}";
                    
                }
            }
            catch (Exception)
            {
                //log exception
                throw;
            }
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await Js.InvokeVoidAsync("initPayPalButton");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }   
}
