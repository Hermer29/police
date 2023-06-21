using System.Collections.Generic;
using ActiveCharacters.Shared.Components;
using Gameplay.ActiveCharacters.Shared.Components.Attacking;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components.OverTimeEffectors
{
    public class ZoneAttackEffector : OverTimeEffector, IDamageValueDependent
    {
        [SerializeField] private ZoneTickDetector _detector;
        
        private float _applyingDamage = .1f;

        public override void StartExecuting(Attackable target)
        {
            _detector.EnablePresenceDetection(target.Root.position);
            _detector.OverlapedTargets += DetectedTargets;
        }

        private void DetectedTargets(IEnumerable<Collider> obj)
        {
            foreach (Collider collided in obj)
            {
                var attackable = collided.GetComponent<Attackable>();
                attackable.ApplyDamage(_applyingDamage);
            }
        }

        public override void EndExecuting()
        {
            _detector.Stop();
            _detector.OverlapedTargets -= DetectedTargets;
        }

        public void InitializeDamage(float damage)
        {
            _applyingDamage = damage;
        }
    }
}