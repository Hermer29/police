using System.Collections;
using Hermer29.Almasury;
using Infrastructure.Loading;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class LoadLevelState : State, ILoadingProcessor
    {
        private readonly ICoroutineRunner _coroutineRunner;

        private bool _levelLoaded;
        
        private const string SceneName = "Town";

        public LoadLevelState(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        
        public float Progress { private set; get; }

        protected override void OnEnter()
        {
            _coroutineRunner.StartCoroutine(LevelLoading());
        }

        private IEnumerator LevelLoading()
        {
            var sceneLoading = SceneManager.LoadSceneAsync(SceneName);
            while (sceneLoading.isDone)
            {
                Progress = sceneLoading.progress;
                yield return null;
            }
            Progress = 1;
        }

        [Transition(typeof(GameplayState))]
        public bool IsLevelLoaded()
        {
            return _levelLoaded;
        }
    }
}