using System;

namespace Services.MoneyService
{
    public interface IMoneyService
    {
        void AddMoney(int amount);
        void SpendMoney(int amount);
        bool TrySpendMoney(int amount);
        int Amount { get; }
        event Action<int> Spent;
        event Action<int> Gained;
    }
}