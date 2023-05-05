using Hermer29.Almasury;
using UnityEngine.SceneManagement;
using DiContainer = Zenject.DiContainer;

namespace Infrastructure.States
{
    public class BootstrapState : State
    {
        private readonly LoadingScreen _loadingScreen;
        
        private bool _isEnded;

        public BootstrapState(DiContainer container)
        {
            _loadingScreen = container.Resolve<LoadingScreen>();
        }
        
        protected override void OnEnter()
        {
            if (SceneManager.GetActiveScene().name != "Bootstrap")
            {
                return;
            }
            _loadingScreen.FadeInImmediately();
            _isEnded = true;
        }

        [Transition(typeof(LoadLevelState))]
        public bool IsEnded()
        {
            return _isEnded;
        }
    }
}