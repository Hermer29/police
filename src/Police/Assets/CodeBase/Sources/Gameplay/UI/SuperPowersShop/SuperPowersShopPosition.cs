using System;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI.SuperPowersShop
{
    [RequireComponent(typeof(Button))]
    public class SuperPowersShopPosition : MonoBehaviour
    {
        private ConsumablesShopService _consumablesService;

        [SerializeField] private ConsumablesShopProduct _product;
        
        [Inject]
        public void Construct(ConsumablesShopService consumablesService)
        {
            _consumablesService = consumablesService;
        }
        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnBuy);
        }

        private void OnBuy() => _consumablesService.Buy(_product);
    }
}