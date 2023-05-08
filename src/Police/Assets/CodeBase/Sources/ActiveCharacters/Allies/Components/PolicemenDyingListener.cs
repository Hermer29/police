using ActiveCharacters.Shared.Components;
using UnityEngine;

namespace ActiveCharacters.Allies.Components
{
    public class PolicemenDyingListener : MonoBehaviour
    {
        [SerializeField] private Attackable _attackable;
        [SerializeField] private PolicemenAggro _aggro;

        private bool _died;
        
        private void Update()
        {
            if (_died)
                return;
            
            if (_attackable.Died)
            {
                _died = true;
                _aggro.Stop();
                _attackable.Root.gameObject.SetActive(false);
            }
        }
    }
}