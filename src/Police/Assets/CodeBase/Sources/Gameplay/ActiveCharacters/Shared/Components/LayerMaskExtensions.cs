using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components
{
    public static class LayerMaskExtensions
    {
        public static bool LayerEquals(Collider other, LayerMask layerMask)
        {
            return 1 << other.gameObject.layer == layerMask;
        }
    }
}