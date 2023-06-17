using Services.MoneyService;
using Services.PurchasesService.PurchasesWrapper;

namespace Infrastructure.Services.YansMoneyExchange
{
    public class YansMoneyService
    {
        private readonly IMoneyService _moneyService;
        private readonly IProductsService _productService;

        public YansMoneyService(IMoneyService moneyService, IProductsService productService)
        {
            _moneyService = moneyService;
            _productService = productService;
        }

        public void BuyMoney(YansMoneyExchangePosition position)
        {
            _productService.TryBuy(position.AsProduct(), () =>
            {
                _moneyService.AddMoney(GetMoneyReward(position));
            });
        }

        private int GetMoneyReward(YansMoneyExchangePosition position) =>
            position switch
            {
                YansMoneyExchangePosition.Coins500 => 500,
                YansMoneyExchangePosition.Coins2000 => 2000,
                YansMoneyExchangePosition.Coins10000 => 10000
            };
    }
}