using Hermer29.Almasury;
using Services;
using DiContainer = Zenject.DiContainer;

namespace Infrastructure.States
{
    public class GameplayState : State
    {
        private readonly DiContainer _container;
        private InputService _inputService;
        
        public GameplayState(DiContainer container)
        {
            _container = container;
        }

        protected override void OnEnter()
        {
            _inputService = _container.Resolve<InputService>();
        }
    }
}