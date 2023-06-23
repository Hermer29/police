using UnityEngine;

namespace Gameplay.ActiveCharacters.Allies.Components
{
    public class VfxForDistanceToTarget : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private RangedPolicemanAggro _rangedPoliceman;
        [SerializeField] private float _ratio;
        
        private void Update()
        {
            if (_rangedPoliceman.TargetDefined == false)
            {
                _particleSystem.gameObject.SetActive(false);
                return;
            }
            _particleSystem.gameObject.SetActive(true);
            var main = _particleSystem.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(_ratio * _rangedPoliceman.TargetDistance);
        }
    }
}