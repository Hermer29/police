using System;
using ActiveCharacters.Shared.Components;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components
{
    public class LookAtTarget : DetectionReaction
    {
        private BoxCollider _target;

        public override void Follow(BoxCollider target)
        {
            _target = target;
        }

        private void Update()
        {
            if(_target == null)
            return;
            transform.LookAt(_target.ClosestPoint(transform.position));
        }

        public override void Forget()
        {
            _target = null;
        }
    }
}