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

        public bool Used { get; set; }

        public void ReturnToPool()
        {
            Used = false;
            _pool.Release(this);
        }

        public void MarkUsed()
        {
            Used = true;
        }
    }
}