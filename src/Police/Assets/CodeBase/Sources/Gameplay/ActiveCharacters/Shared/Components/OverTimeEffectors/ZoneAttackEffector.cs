using System;
using System.Collections.Generic;
using ActiveCharacters.Shared.Components;
using Gameplay.ActiveCharacters.Shared.Components.Attacking;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components.OverTimeEffectors
{
    public class ZoneAttackEffector : OverTimeEffector
    {
        [SerializeField] private ZoneTickDetector _detector;
        [SerializeField] private Attacker _attacker;

        private bool _started;

        private void Start()
        {
            _detector.OverlapedTargets += DetectedTargets;
        }

        public override void StartExecuting(Attackable target)
        {
            if (_started)
                return;
            _started = false;
            _detector.EnablePresenceDetection(target.Root.position);
        }

        private void DetectedTargets(IEnumerable<Collider> obj)
        {
            foreach (Collider collided in obj)
            {
                var attackable = collided.GetComponent<Attackable>();
                _attacker.ApplyDamage(attackable);
            }
        }

        public override void EndExecuting()
        {
            _started = false;
            _detector.Stop();
        }
    }
}