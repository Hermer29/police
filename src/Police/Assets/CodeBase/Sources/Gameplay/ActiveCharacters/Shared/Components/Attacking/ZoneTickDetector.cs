using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components.Attacking
{
    public class ZoneTickDetector : MonoBehaviour
    {
        [SerializeField] private float _height;
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private float _tickTime;
        
        private Collider[] _colliders = new Collider[70];
        private Vector3 _affectedPoint;
        private Coroutine _worker;
        private bool _enabled;

        public event Action<IEnumerable<Collider>> OverlapedTargets;
        
        public void EnablePresenceDetection(Vector3 point)
        {
            if (_enabled)
                return;
            _enabled = true;
            _affectedPoint = point;
            _worker = StartCoroutine(Worker());
        }

        private IEnumerable<Collider> OverlapCapsule()
        {
            int size = Physics.OverlapCapsuleNonAlloc(_affectedPoint, _affectedPoint + Vector3.up * _height, _radius, _colliders, _mask);
            return size == 0 ? Enumerable.Empty<Collider>() : _colliders.Where(x => x != null);
        }

        private IEnumerator Worker()
        {
            while(true)
            {
                if(_enabled == false)
                    yield break;
                var targets = OverlapCapsule();
                if (targets.Any())
                {
                    OverlapedTargets?.Invoke(targets);
                }
                yield return new WaitForSeconds(_tickTime);
            }
        }

        public void Stop()
        {
            _enabled = false;
            StopAllCoroutines();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + Vector3.right * 10, _radius);
        }
    }
}