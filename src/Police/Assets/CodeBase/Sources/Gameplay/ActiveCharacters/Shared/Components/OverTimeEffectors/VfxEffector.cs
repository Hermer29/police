using ActiveCharacters.Shared.Components;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components.OverTimeEffectors
{
    public class VfxEffector : OverTimeEffector
    {
        [SerializeField] private GameObject _vfx;

        private void Start() => EndExecuting();

        public override void StartExecuting(Attackable target) => _vfx.SetActive(true);

        public override void EndExecuting() => _vfx.SetActive(false);
    }
}