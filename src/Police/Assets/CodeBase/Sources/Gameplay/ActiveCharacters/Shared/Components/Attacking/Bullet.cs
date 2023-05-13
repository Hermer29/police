using System;
using Logic;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components.Attacking
{
    public class Bullet : MonoBehaviour
    {
        private PoolHandle _poolHandle;
        
        [SerializeField] private float _speed;

        private Transform _target;
        private Action _onHit;

        public void Construct(PoolHandle handle)
        {
            _poolHandle = handle;
        }
        
        public void ShotTowards(Transform target, Action onHit)
        {
            _onHit = onHit;
            _target = target;
        }

        private void Update()
        {
            if (_target == null)
                return;

            transform.position = Vector3.MoveTowards(
                current: transform.position, 
                target: _target.position, 
                maxDistanceDelta: _speed * Time.deltaTime);

            if (transform.position == _target.position)
            {
                OnHit();
            }
        }

        private void OnHit()
        {
            _onHit?.Invoke();
            _target = null;
            _poolHandle.Free();
            _onHit = null;
        }
    }
}