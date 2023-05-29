using UnityEngine;

namespace AnimationUtility
{
    public abstract class Animation : MonoBehaviour
    {
        public abstract void Stop();
        public abstract void Play();

        private void OnDisable()
        {
            Stop();
        }
    }
}