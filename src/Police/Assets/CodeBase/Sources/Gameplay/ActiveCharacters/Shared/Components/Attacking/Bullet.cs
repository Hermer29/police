using System;
using ActiveCharacters.Shared.Components;
using Logic;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components.Attacking
{
    public class Bullet : MonoBehaviour
    {
        private PoolHandle _poolHandle;

        [SerializeField] private TrailRenderer _trails;
        [SerializeField] private float _speed;

        private Attackable _target;
        private Action _onHit;

        public void Construct(PoolHandle handle)
        {
            _poolHandle = handle;
        }
        
        public void ShotTowards(Attackable target, Action onHit)
        {
            _onHit = onHit;
            _target = target;
            FlushTrail();
        }

        private void Update()
        {
            if (_target == null)
                return;
            
            if(_target.Died)
            {
                OnHit();
                return;
            }

            transform.position = Vector3.MoveTowards(
                current: transform.position, 
                target: _target.Root.position, 
                maxDistanceDelta: _speed * Time.deltaTime);

            if (transform.position == _target.Root.position)
            {
                OnHit();
            }
        }
        
        public void ForceFree()
        {
            _target = null;
        }

        private void OnHit()
        {
            _onHit?.Invoke();
            _target = null;
            _poolHandle.Free();
            _onHit = null;
        }

        private void FlushTrail()
        {
            _trails.Clear();
        }
    }
}