using System;
using Services.PrefsService;
using UnityEngine;

namespace Services.AdvertisingService.AdBlocking
{
    public class BlockingAdsAdvertisingDecorator : IAdvertisingService
    {
        private readonly IAdvertisingService _advertisingService;
        private readonly IPrefsService _prefsService;
        
        private const string AdvertisingDisabledKey = "AdvertisingDisabled";

        public BlockingAdsAdvertisingDecorator(IAdvertisingService advertisingService, IPrefsService prefsService)
        {
            _advertisingService = advertisingService;
            _prefsService = prefsService;
        }

        public bool IsAdvertisingDisabled 
            => _prefsService.GetInt(AdvertisingDisabledKey, 0) == 1;

        public void DisableAdvertising() 
            => _prefsService.SetInt(AdvertisingDisabledKey, 1);

        public void ShowInterstitial()
        {
            Debug.Log($"{nameof(BlockingAdsAdvertisingDecorator)} called, {nameof(IsAdvertisingDisabled)}: {IsAdvertisingDisabled}");

            if (IsAdvertisingDisabled)
                return;
            _advertisingService.ShowInterstitial();
        }

        public void ShowRewarded(Action onClosedAndRewarded, Action onError = null) 
            => _advertisingService.ShowRewarded(onClosedAndRewarded, onError);
    }
}