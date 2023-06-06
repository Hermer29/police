using System.Collections;
using Gameplay.Levels.Services.LevelsTracking;
using Gameplay.Levels.UI.CrossLevelUi;
using Gameplay.UI;
using Helpers.UI;
using Hermer29.Almasury;
using UnityEngine;
using DiContainer = Zenject.DiContainer;

namespace Infrastructure.States
{
    public class MenuState : State
    {
        private CrossLevelUi _crossLevelUi;
        private MoneyUi _moneyUi;
        private ILevelService _levelService;
        private readonly ICoroutineRunner _coroutineRunner;

        private bool _goingToPlay;

        public MenuState(ICoroutineRunner coroutineRunner) => _coroutineRunner = coroutineRunner;

        private void ResolveDependencies()
        {
            var container = AllServices.Get<DiContainer>();
            _crossLevelUi = container.Resolve<CrossLevelUi>();
            _moneyUi = container.Resolve<MoneyUi>();
            _levelService = container.Resolve<ILevelService>();
        }
        
        protected override void OnEnter()
        {
            Debug.Log($"{nameof(MenuState)}.{nameof(OnEnter)} called");
            ResolveDependencies();

            _moneyUi.Show();
            _coroutineRunner.StartCoroutine(Work());
        }

        private IEnumerator Work()
        {
            var showCommands = new CrossLevelUiShowCommand(_crossLevelUi, _levelService);
            showCommands.Show();
            yield return new WaitForButtonClick(_crossLevelUi.PlayButton);
            _goingToPlay = true;
        }

        protected override void OnExit()
        {
            _goingToPlay = false;
            _crossLevelUi.Hide();
            _moneyUi.Hide();
        }

        [Transition(typeof(GameplayState))]
        public bool IsGoingToPlay() => _goingToPlay;
    }
}