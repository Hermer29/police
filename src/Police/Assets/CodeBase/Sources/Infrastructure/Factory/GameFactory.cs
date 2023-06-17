using Gameplay.Levels.UI.CrossLevelUi;
using Helpers;
using Infrastructure.AssetManagement;
using Tutorial;
using UnityEngine;
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

        public GameObject CreateNuke() => 
            Object.Instantiate(_assets.LoadNuke());
        
        public async void CreateNukeFx(Vector3 at) =>
            Object.Instantiate(await _assets.LoadNukeFx())
                .WithPosition(at);
    }
}