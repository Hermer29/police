using System.Collections;
using Logic;
using UnityEngine;

namespace Fx
{
    public class ParticlesHolder : MonoBehaviour
    {
        private PoolHandle _poolHandle;
        
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private float _playtime;

        public void Construct(PoolHandle poolHandle)
        {
            _poolHandle = poolHandle;
        }
        
        public void Play() => StartCoroutine(Waiting());

        private IEnumerator Waiting()
        {
            _particles.Play(true);
            yield return new WaitForSeconds(_playtime);
            Deinitialize();
        }

        private void Deinitialize()
        {
            _particles.Stop();
            _poolHandle.Free();
        }

        public void Stop()
        {
        }
    }
}