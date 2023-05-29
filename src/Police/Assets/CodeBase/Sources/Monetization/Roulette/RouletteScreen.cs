using Interface;
using UnityEngine;

namespace Monetization
{
    public class RouletteScreen : MonoBehaviour
    {
        [SerializeField] private RouletteArrow _arrow;
    
        public void StartRoulette() =>
            _arrow.RotateСontinuously();

        public float StopRoulette() => 
            _arrow.StopRotation();

        public int GetIndex(float rotationInRadians) => 
            _arrow.GetPrizeIndex(rotationInRadians);
    }
}