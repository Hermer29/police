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
        private const int UpdateFrequency = 2;
        [SerializeField] private Color _color;
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private LayerMask _mask;
        
        private bool _triggered;
        private readonly Collider[] _overlapBuffer = new Collider[10];
        private Coroutine _targetsListening;

        public event Action<Collider> TriggerEntered;

        public IEnumerable<Collider> Overlap()
        {
            Physics.OverlapSphereNonAlloc(SphereCenter(), _collider.radius, _overlapBuffer, _mask);
            return _overlapBuffer.Where(x => x != null).Where(x => x.gameObject.activeInHierarchy);
        }

        private void OnEnable()
        {
            _targetsListening = StartCoroutine(OverlapOverTime());
        }

        private void OnDisable()
        {
            StopCoroutine(_targetsListening);
        }

        private IEnumerator OverlapOverTime()
        {
            while (true)
            {
                foreach (Collider col in Overlap())
                {
                    OnTriggerEnter(col);
                }

                yield return new WaitForSeconds(UpdateFrequency);
            }
        }

        private Vector3 SphereCenter()
        {
            return transform.position + _collider.center;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (LayerMaskExtensions.LayerEquals(other, _mask) == false)
                return;
            
            TriggerEntered?.Invoke(other);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(_collider.center + transform.position, _collider.radius);
        }
    }
}