using UnityEngine;

namespace Gameplay.UI
{
    internal class BarracksSubwindow : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}