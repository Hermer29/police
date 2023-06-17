using Infrastructure.Services.YansMoneyExchange;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class YansMoneyExchangeShopPosition : MonoBehaviour
    {
        private YansMoneyService _consumablesService;

        [SerializeField] private YansMoneyExchangePosition _product;
        
        [Inject]
        public void Construct(YansMoneyService consumablesService)
        {
            _consumablesService = consumablesService;
        }
        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnBuy);
        }

        private void OnBuy() => _consumablesService.BuyMoney(_product);
    }
}