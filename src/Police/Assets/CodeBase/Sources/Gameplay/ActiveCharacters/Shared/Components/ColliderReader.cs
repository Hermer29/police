using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameplay.ActiveCharacters.Shared.Components;
using UnityEngine;

namespace ActiveCharacters.Shared.Components
{
    public class ColliderReader : MonoBehaviour
    {
        [SerializeField] private Color _color;
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private LayerMask _mask;
        
        private bool _triggered;
        private readonly Collider[] _overlapBuffer = new Collider[10];
        
        public event Action<Collider> TriggerEntered;
        public event Action<Collider> TriggerExit;

        public IEnumerable<Collider> Overlap()
        {
            Physics.OverlapSphereNonAlloc(SphereCenter(), _collider.radius, _overlapBuffer, _mask);
            return _overlapBuffer.Where(x => x != null).Where(x => x.gameObject.activeInHierarchy);
        }

        private Vector3 SphereCenter()
        {
            return transform.position + _collider.center;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered)
                return;

            if (LayerMaskExtensions.LayerEquals(other, _mask) == false)
                return;
            
            _triggered = true;
            TriggerEntered?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_triggered == false)
                return;
            
            _triggered = false;
            TriggerExit?.Invoke(other);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(_collider.center + transform.position, _collider.radius);
        }
    }
}