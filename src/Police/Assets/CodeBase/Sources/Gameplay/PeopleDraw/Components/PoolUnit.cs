using UnityEngine;
using UnityEngine.Pool;

namespace PeopleDraw.Components
{
    public class PoolUnit : MonoBehaviour
    {
        [field: SerializeField] public PlacingBlock PlacingBlock { get; private set; }

        private ObjectPool<PoolUnit> _pool;

        public void Construct(ObjectPool<PoolUnit> pool)
        {
            _pool = pool;
        }

        public bool Used { get; set; }

        public void ReturnToPool() => Release();

        private void Release()
        {
            Used = false;
            if (_pool == null)
                return;
            _pool.Release(this);
        }

        public void MarkUsed() => Used = true;
    }
}