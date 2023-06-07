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
            
            var loadResourcesState = new LoadGameResources(this);
            var loadLevelState = new LoadLevelState(coroutineRunner: this, _diContainer, loadResourcesState);

            Container.Add(loadLevelState);
            Container.Add(loadResourcesState);
            Container.Add<SdkInitializationState>();
        }
    }
}