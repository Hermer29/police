using Gameplay.Levels.UI.CrossLevelUi;
using Tutorial;
using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public class AssetLoader : IAssetLoader
    {
        private const string PrefabsUICrossLevelUi = "Prefabs/UI/CrossLevelUi";
        private const string TutorialPath = "Prefabs/UI/Tutorial";

        public CrossLevelUi LoadCrossLevelUI() => Resources.Load<CrossLevelUi>(PrefabsUICrossLevelUi);

        public TutorialEngine LoadTutorial() => Resources.Load<TutorialEngine>(TutorialPath);
    }
}