using UnityEngine;

namespace Gameplay.UI
{
    internal class ShopSubwindow : MonoBehaviour
    {
        public void Shop() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}