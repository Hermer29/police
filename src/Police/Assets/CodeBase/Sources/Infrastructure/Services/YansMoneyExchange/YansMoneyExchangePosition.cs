using System;
using Services.PurchasesService.PurchasesWrapper;

namespace Infrastructure.Services.YansMoneyExchange
{
    public enum YansMoneyExchangePosition
    {
        Coins500,
        Coins2000,
        Coins10000
    }

    public static class YansMoneyExchangePositionExtensions
    {
        public static Product AsProduct(this YansMoneyExchangePosition position)
        {
            return position switch
            {
                YansMoneyExchangePosition.Coins500 => Product.Coins500,
                YansMoneyExchangePosition.Coins2000 => Product.Coins2000,
                YansMoneyExchangePosition.Coins10000 => Product.Coins10000, 
                _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
            };
        }
    }
}