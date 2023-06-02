using Interface;
using UnityEngine;

namespace Monetization
{
    public class RouletteScreen : MonoBehaviour
    {
        [SerializeField] private RouletteArrow _arrow;
    
        public void StartRoulette() =>
            _arrow.RotateСontinuously();

        public void Stop() => 
            _arrow.StopRotation();

        public float CurrentRotation() => _arrow.CurrentRotation;

        public int GetPrizeIndex() => _arrow.GetPrizeIndex();
    }
}