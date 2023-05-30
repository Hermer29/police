using System;
using Services.PrefsService;
using UnityEngine;

namespace Services.MoneyService
{
    public class MoneyService : IMoneyService
    {
        private readonly IPrefsService _prefsService;

        private const string MoneyKey = "Money";

        public MoneyService(IPrefsService prefsService) 
            => _prefsService = prefsService;

        public int Amount => MoneyAmount;

        public event Action<int> Spent;
        public event Action<int> Gained;

        private int MoneyAmount
        {
            get => _prefsService.GetInt(MoneyKey);
            set => _prefsService.SetInt(MoneyKey, value);
        }

        public void AddMoney(int amount)
        {
            Debug.Log($"{nameof(MoneyService)}.{nameof(AddMoney)} called. Added {amount}, now: {MoneyAmount + amount}");

            MoneyAmount += amount;
            Gained?.Invoke(amount);
        }

        public void SpendMoney(int amount)
        {
            if (TrySpendMoney(amount) == false)
                throw new InvalidOperationException("Insufficient funds");
        }

        public bool TrySpendMoney(int amount)
        {
            var notEnoughCoins = MoneyAmount < amount;
            Debug.Log($"{nameof(MoneyService)}.{nameof(TrySpendMoney)} called. " +
                      $"{(notEnoughCoins ? "Insufficient funds" : "Success")}. " +
                      $"Prev: {MoneyAmount}, Now: {MoneyAmount - amount}");
            if(notEnoughCoins)
                return false;

            MoneyAmount -= amount;
            Spent?.Invoke(amount);
            return true;
        }
    }
}