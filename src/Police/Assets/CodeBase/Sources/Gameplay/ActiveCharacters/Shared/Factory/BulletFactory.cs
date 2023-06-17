using System.Collections.Generic;
using Gameplay.ActiveCharacters.Shared.Components.Attacking;
using Logic;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.PeopleDraw.Factory
{
    public class BulletFactory : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletsParent;
        
        private ObjectPool<Bullet> _pool;
        private List<Bullet> _unfreeBullet = new List<Bullet>();

        private void Start()
        {
            _pool = new ObjectPool<Bullet>(
                createFunc: CreateFunc, 
                actionOnGet: bullet => bullet.gameObject.SetActive(true),
                actionOnRelease:  ActionOnRelease);
        }

        public void FreeAll()
        {
            foreach (Bullet bullet in _unfreeBullet.ToArray())
            {
                bullet.ForceFree();
                _pool.Release(bullet);
            }
        }

        private void ActionOnRelease(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _unfreeBullet.Remove(bullet);
        }

        public Bullet CreateBullet(Vector3 muzzlePosition)
        {
            var bullet = _pool.Get();
            _unfreeBullet.Add(bullet);
            bullet.transform.position = muzzlePosition;
            return bullet;
        }

        private Bullet CreateFunc()
        {
            Bullet bullet = Instantiate(_bulletPrefab, _bulletsParent);
            bullet.Construct(new PoolHandle(() => _pool.Release(bullet)));
            return bullet;
        }
    }
}