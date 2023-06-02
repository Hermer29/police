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
            DontDestroyOnLoad(gameObject);
            _diContainer = container;
        }
            
        protected override void Install() => BindStates();

        private void BindStates()
        {
            AllServices.Bind<ICoroutineRunner>(this);
            
            Container.Add<ICoroutineRunner>(this);
            Container.Add(_diContainer);
            
            Container.Add(new BootstrapState(_diContainer)).AsFirstState();
            Container.Add<GameplayState>(new GameplayState());
            Container.Add<CreateServicesState>(new CreateServicesState());
            Container.Add<MenuState>(new MenuState(this));
            
            var loadLevelState = new LoadLevelState(coroutineRunner: this, _diContainer);
            var loadResourcesState = new LoadGameResources(this);
            RequestLoading(loadLevelState, loadResourcesState);
            
            Container.Add(loadLevelState);
            Container.Add(loadResourcesState);
            Container.Add<SdkInitializationState>();
        }

        private void RequestLoading(params ILoadingProcessor[] loadingProcessors)
        {
            var loadings = _diContainer.Resolve<LoadingManager>();
            loadings.RequestLoadingImmediately(loadingProcessors);
        }
    }
}