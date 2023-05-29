using Gameplay.Levels.UI.CrossLevelUi;
using Infrastructure.AssetManagement;
using Tutorial;
using Zenject;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetLoader _assets;

        public GameFactory(IAssetLoader assets) => _assets = assets;

        public CrossLevelUi CreateCrossLevelUI() => 
            AllServices.Get<DiContainer>()
                .InstantiatePrefabForComponent<CrossLevelUi>(
                    _assets.LoadCrossLevelUI());

        public TutorialEngine CreateTutorial() => 
            AllServices.Get<DiContainer>()
                .InstantiatePrefabForComponent<TutorialEngine>(
                    _assets.LoadTutorial());
    }
}