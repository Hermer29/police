using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PeopleDraw.EnergyConsumption
{
    public class EnergyUi : MonoBehaviour
    {
        [SerializeField] private TMP_Text _energyCounter;
        [SerializeField] private Slider _energySlider;

        private int _maxValue;
        
        public void DefineMaxValue(int amount)
        {
            _maxValue = amount;
            DrawSliderForAmount(amount);
            DrawEnergyCounter(amount);
        }

        public void SetEnergyValue(int amount)
        {
            DrawEnergyCounter(amount);
            DrawSliderForAmount(amount);
        }

        private void DrawSliderForAmount(int amount) => _energySlider.value = (float) amount / _maxValue;

        private void DrawEnergyCounter(int amount) => _energyCounter.text = $"{amount}/{_maxValue}";
    }
}