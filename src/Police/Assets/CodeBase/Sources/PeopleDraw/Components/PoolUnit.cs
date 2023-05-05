using UnityEngine;
using UnityEngine.Pool;

namespace PeopleDraw.Components
{
    public class PoolUnit : MonoBehaviour
    {
        private ObjectPool<PoolUnit> _pool;
        
        public void Construct(ObjectPool<PoolUnit> pool)
        {
            _pool = pool;
        }

        public void ReturnToPool()
        {
            _pool.Release(this);
        }
    }
}