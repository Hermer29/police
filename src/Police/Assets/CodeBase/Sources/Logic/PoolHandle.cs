using System;

namespace Logic
{
    public class PoolHandle
    {
        private readonly Action _onFree;

        public PoolHandle(Action onFree)
        {
            _onFree = onFree;
        }
        
        public void Free()
        {
            _onFree.Invoke();
        }
    }
}