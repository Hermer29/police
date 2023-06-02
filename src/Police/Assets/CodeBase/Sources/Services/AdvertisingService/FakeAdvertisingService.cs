using System;
using UnityEngine;

namespace Services.AdvertisingService
{
    public class FakeAdvertisingService : IAdvertisingService
    {
        public void ShowInterstitial()
        {
            Debug.Log($"{nameof(FakeAdvertisingService)}.{nameof(ShowInterstitial)} called");
        }

        public void ShowRewarded(Action onClosedAndRewarded)
        {
            Debug.Log($"{nameof(FakeAdvertisingService)}.{nameof(ShowRewarded)} called");
            onClosedAndRewarded.Invoke();
        }
    }
}