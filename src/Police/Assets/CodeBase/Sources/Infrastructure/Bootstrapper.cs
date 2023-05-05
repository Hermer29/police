using Hermer29.Almasury;
using Infrastructure.Loading;
using Infrastructure.States;
using Zenject;
using DiContainer = Zenject.DiContainer;

namespace Infrastructure
{
    public class Bootstrapper : StateMachineInstaller, ICoroutineRunner
    {
        private DiContainer _diContainer;

        [Inject]
        public void Construct(DiContainer container)
        {
            _diContainer = container;
        }
            
        protected override void Install()
        {
            BindStates();
        }

        private void BindStates()
        {
            Container.Add(new BootstrapState(
                    container: _diContainer))
                .AsFirstState();

            var loadLevelState = new LoadLevelState(
                coroutineRunner: this);
            
            Container.Add(loadLevelState);

            Container.Add(new GameplayState(
                 container: _diContainer));
            
            RequestLoading(loadLevelState);
        }

        private void RequestLoading(params ILoadingProcessor[] loadingProcessors)
        {
            var loadings = _diContainer.Resolve<LoadingManager>();
            loadings.RequestLoadingImmediately(loadingProcessors);
        }
    }
}