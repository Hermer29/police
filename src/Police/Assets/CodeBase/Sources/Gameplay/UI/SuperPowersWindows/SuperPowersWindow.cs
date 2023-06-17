using Services.SuperPowersService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class SuperPowersWindow : MonoBehaviour
    {
        private SuperPowers _powers;
        private SuperPowersUi _superPowersUi;
        
        [SerializeField] private SuperPower _superPower;
        
        [SerializeField] private Button _buyForYans;
        [SerializeField] private Button _buyForAds;
        [SerializeField] private Button _consumeAccepting;
        [SerializeField] private TMP_Text _consumableQuantity;
        
        public void Construct(SuperPowers powers, SuperPowersUi superPowersUi)
        {
            _powers = powers;
            _superPowersUi = superPowersUi;
        }

        public void Show()
        {
            _superPowersUi.ShowWindows();
            gameObject.SetActive(true);
            if (TryIntroduceOptionToConsume(_superPower))
            {
                return;
            }
            _buyForAds.onClick.AddListener(
                () => _powers.UseSuperPowerForAdvertising(onSuccess: _superPowersUi.Close, superPower: _superPower));
            _buyForYans.onClick.AddListener(
                () => _powers.UseSuperPowerForYans(onSuccess: _superPowersUi.Close, superPower: _superPower));
        }
        
        private bool TryIntroduceOptionToConsume(SuperPower superPower)
        {
            if (_powers.CanConsumeSuperPower(superPower))
            {
                ConsumePower(superPower: superPower);
                return true;
            }
            _buyForAds.gameObject.SetActive(true);
            _buyForYans.gameObject.SetActive(true);
            _consumeAccepting.gameObject.SetActive(false);
            return false;
        }

        private void ConsumePower(SuperPower superPower)
        {
            _consumeAccepting.gameObject.SetActive(true);
            _buyForAds.gameObject.SetActive(false);
            _buyForYans.gameObject.SetActive(false);
            _consumableQuantity.text = "x" + _powers.SuperPowerQuantity(superPower);
            _consumeAccepting.onClick.AddListener(() =>
            {
                _powers.ConsumeSuperPower(superPower);
                _superPowersUi.Close();
            });
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}