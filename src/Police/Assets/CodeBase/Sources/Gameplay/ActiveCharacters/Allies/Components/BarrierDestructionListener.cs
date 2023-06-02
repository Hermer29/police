using ActiveCharacters.Shared.Components;
using PeopleDraw.Components;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Allies.Components
{
    public class BarrierDestructionListener : MonoBehaviour
    {
        [SerializeField] private Attackable _attackable;
        [SerializeField] private PoolUnit _poolUnit;
        
        private bool _died;
        
        private void Update()
        {
            if (_died)
                return;
            
            if (_attackable.Died)
            {
                _died = true;
                _poolUnit.ReturnToPool();
            }
        }
    }
}