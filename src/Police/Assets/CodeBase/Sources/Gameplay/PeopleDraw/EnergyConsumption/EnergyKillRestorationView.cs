using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.PeopleDraw.EnergyConsumption
{
    public class EnergyKillRestorationView : MonoBehaviour
    {
        private Camera _camera;
        
        [SerializeField] private EnergyKillRestorationViewItem _view;
        [SerializeField] private float _fxSpawnYOffset;

        private Lazy<ObjectPool<EnergyKillRestorationViewItem>> _pool;

        private void Start()
        {
            _camera = Camera.main;
            
            _pool = new Lazy<ObjectPool<EnergyKillRestorationViewItem>>(
                valueFactory: () => new ObjectPool<EnergyKillRestorationViewItem>(
                    createFunc: () => Instantiate(_view, transform)));
        }

        public void Show(Transform enemy)
        {
            Vector3 worldPoint = enemy.position;
            Vector3 screenPoint = _camera.WorldToScreenPoint(worldPoint);
            EnergyKillRestorationViewItem value = _pool.Value.Get();
            value.transform.position = screenPoint + Vector3.up * _fxSpawnYOffset;
            value.Show(() => _pool.Value.Release(value));
        }
    }
}