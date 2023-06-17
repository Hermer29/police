using System;
using Services.AdvertisingService;
using Services.PropertyAcquisition;
using Services.PurchasesService.PurchasesWrapper;

namespace Services.SuperPowersService
{
    public class SuperPowers
    {
        private readonly PropertyService _propertyService;
        private readonly IAdvertisingService _advertisingService;
        private readonly Nuke _nuke;
        private readonly SuperUnit _superUnit;
        private readonly IProductsService _productsService;

        public SuperPowers(PropertyService propertyService, IAdvertisingService advertisingService, Nuke nuke, IProductsService productsService,
            SuperUnit superUnit)
        {
            _propertyService = propertyService;
            _advertisingService = advertisingService;
            _nuke = nuke;
            _productsService = productsService;
            _superUnit = superUnit;
        }

        public void UseNukeForAdvertising(Action onSuccess)
        {
            _advertisingService.ShowRewarded(() =>
            {
                LaunchNuke();
                onSuccess();
            });
        }

        public void UseSuperUnitForAdvertising(Action onSuccess)
        {
            _advertisingService.ShowRewarded(() =>
            {
                MakeSuperUnit();
                onSuccess();
            });
        }

        public void UseSuperUnitForYans(Action onSuccess)
        {
            _productsService.TryBuy(Product.SuperUnit1, () =>
            {
                MakeSuperUnit();
                onSuccess.Invoke();
            });
        }

        public void UseNukeForYans(Action onSuccess)
        {
            _productsService.TryBuy(Product.Nuke1, () =>
            {
                LaunchNuke();
                onSuccess.Invoke();
            });
        }

        public void ConsumeSuperPower(SuperPower power)
        {
            _propertyService.Consume(power.AsProperty());
            switch (power)
            {
                case SuperPower.Nuke:
                    LaunchNuke();
                    break;
                case SuperPower.SuperUnit:
                    MakeSuperUnit();
                    break;
            }
        }

        public void UseSuperPowerForAdvertising(SuperPower superPower, Action onSuccess)
        {
            if (superPower == SuperPower.Nuke)
            {
                UseNukeForAdvertising(onSuccess);
            }
            else if (superPower == SuperPower.SuperUnit)
            {
                UseSuperUnitForAdvertising(onSuccess);
            }
        }

        public void UseSuperPowerForYans(SuperPower superPower, Action onSuccess)
        {
            if (superPower == SuperPower.Nuke)
            {
                UseNukeForYans(onSuccess);
            }
            else if (superPower == SuperPower.SuperUnit)
            {
                UseSuperUnitForYans(onSuccess);
            }
        }
        
        public bool CanConsumeSuperPower(SuperPower power) 
            => _propertyService.GetQuantity(power.AsProperty()) != 0;

        public int SuperPowerQuantity(SuperPower power) => _propertyService.GetQuantity(power.AsProperty());

        private void LaunchNuke()
        {
            _nuke.LaunchNuke();
        }

        private void MakeSuperUnit()
        {
            _superUnit.Summon();
        }
    }
}