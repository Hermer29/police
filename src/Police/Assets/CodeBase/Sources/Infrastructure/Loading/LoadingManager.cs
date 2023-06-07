using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Infrastructure.Loading
{
    public class LoadingManager : MonoBehaviour
    {
        private LoadingScreen _loadingScreen;
        
        private ILoadingProcessor[] _currentProcessors;

        [Inject]
        public void Construct(LoadingScreen loadingScreen) 
            => _loadingScreen = loadingScreen;

        public void RequestLoadingImmediately(params ILoadingProcessor[] processors)
        {
            ThrowIfAlreadyLoading();
            
            _loadingScreen.FadeInImmediately();
            RequestLoadingActually(processors);
        }

        public void RequestLoading(params ILoadingProcessor[] processors)
        {
            ThrowIfAlreadyLoading();
            
            _loadingScreen.FadeIn();
            RequestLoadingActually(processors);
        }

        private void RequestLoadingActually(ILoadingProcessor[] processors)
        {
            _currentProcessors = processors;
            StartCoroutine(WatchForProcessors());
        }

        private void ThrowIfAlreadyLoading()
        {
            if (_currentProcessors != null)
                throw new InvalidOperationException("Already loading");
        }

        private IEnumerator WatchForProcessors()
        {
            Assert.IsNotNull(_currentProcessors);
            Debug.Log("Start loading");
            
            while (true)
            {
                float overallCompletion = _currentProcessors.Average(x => x.Progress);
                _loadingScreen.SetSliderProcess(overallCompletion);
                if (Math.Abs(overallCompletion - 1) < Mathf.Epsilon)
                {
                    break;
                }
                yield return null;
            }
            Debug.Log("Requested loading completed");
            _loadingScreen.FadeOut();
            foreach (ILoadingProcessor processor in _currentProcessors)
            {
                processor.Progress = 0;
            }
            _currentProcessors = null;
        }
    }
}