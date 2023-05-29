

using UnityEngine;

namespace Interface
{
    /// <summary>
    /// Выключает или включает UI объекты если это задумано логикой
    /// </summary>
    public class GameObjectActivationBaker : MonoBehaviour
    {
        [SerializeField] private bool _bakedActive;

        private void Start()
        {
            SetActive(_bakedActive);
        }

        public void SetActive(bool active)
        {
            _bakedActive = active;
            gameObject.SetActive(_bakedActive);
        }
    }
}