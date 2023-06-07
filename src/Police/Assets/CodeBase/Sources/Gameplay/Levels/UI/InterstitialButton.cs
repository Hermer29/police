using Services.AdvertisingService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Levels.UI
{
    [RequireComponent(typeof(Button))]
    public class InterstitialButton : MonoBehaviour
    {
        [Inject]
        private void Construct(IAdvertisingService advertisingService)
            => GetComponent<Button>().onClick.AddListener(advertisingService.ShowInterstitial);
    }
}