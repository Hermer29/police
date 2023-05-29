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

        private void Start()
        {
            _pool = new ObjectPool<Bullet>(
                createFunc: CreateFunc, 
                actionOnGet: bullet => bullet.gameObject.SetActive(true),
                actionOnRelease:  bullet => bullet.gameObject.SetActive(false));
        }

        public Bullet CreateBullet(Vector3 muzzlePosition)
        {
            var bullet = _pool.Get();
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