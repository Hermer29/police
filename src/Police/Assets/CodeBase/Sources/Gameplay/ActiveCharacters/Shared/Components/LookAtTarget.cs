using System;
using ActiveCharacters.Shared.Components;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components
{
    public class LookAtTarget : DetectionReaction
    {
        private Transform _target;

        public override void Follow(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            transform.LookAt(_target);
        }

        public override void Forget()
        {
            _target = null;
        }
    }
}