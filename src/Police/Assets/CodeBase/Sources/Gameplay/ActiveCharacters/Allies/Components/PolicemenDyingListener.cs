using ActiveCharacters.Shared.Components;
using PeopleDraw.Components;
using UnityEngine;

namespace ActiveCharacters.Allies.Components
{
    public class PolicemenDyingListener : MonoBehaviour
    {
        [SerializeField] private Attackable _attackable;
        [SerializeField] private PolicemenAggro _aggro;
        [SerializeField] private PoolUnit _pool;

        private bool _died;
        
        private void Update()
        {
            if (_died)
                return;
            
            if (_attackable.Died)
            {
                _pool.ReturnToPool();
                _died = true;
                _aggro.Stop();
            }
        }
    }
}