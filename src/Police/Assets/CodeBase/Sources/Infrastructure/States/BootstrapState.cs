using System;
using System.Collections;
using Hermer29.Almasury;
using Infrastructure.Services.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using DiContainer = Zenject.DiContainer;

namespace Infrastructure.States
{
    public class BootstrapState : State
    {
        private readonly LoadingScreen _loadingScreen;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly LocalizationService _localizationInitialization;
        
        private bool _isEnded;

        public BootstrapState(DiContainer container)
        {
            _loadingScreen = container.Resolve<LoadingScreen>();
            _localizationInitialization = container.Resolve<LocalizationService>();
            _coroutineRunner = AllServices.Get<ICoroutineRunner>();
        }
        
        protected override void OnEnter()
        {
            _coroutineRunner.StartCoroutine(WaitForLocalizationLoad(continuation: () =>
            {
                if (SceneManager.GetActiveScene().name != "Bootstrap")
                {
                    return;
                }
                _loadingScreen.FadeInImmediately();
                _isEnded = true;
            }));
        }

        private IEnumerator WaitForLocalizationLoad(Action continuation)
        {
            yield return _localizationInitialization.Initialize();
            continuation.Invoke();
        }

        [Transition(typeof(SdkInitializationState))]
        public bool IsEnded() => _isEnded;
    }
}