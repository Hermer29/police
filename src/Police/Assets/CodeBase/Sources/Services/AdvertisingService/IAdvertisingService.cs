using System;

namespace Services.AdvertisingService
{
    public interface IAdvertisingService
    {
        void ShowInterstitial();
        void ShowRewarded(Action onClosedAndRewarded, Action onError = null);
    }
}