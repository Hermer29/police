using System.Threading.Tasks;
using Gameplay.Levels.UI.CrossLevelUi;
using Tutorial;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.AssetManagement
{
    public class AssetLoader : IAssetLoader
    {
        private const string PrefabsUICrossLevelUi = "Prefabs/UI/CrossLevelUi";
        private const string TutorialPath = "Prefabs/UI/Tutorial";
        private const string NukePath = "Prefabs/Nuke";

        public CrossLevelUi LoadCrossLevelUI() => Resources.Load<CrossLevelUi>(PrefabsUICrossLevelUi);

        public TutorialEngine LoadTutorial() => Resources.Load<TutorialEngine>(TutorialPath);
        public GameObject LoadNuke() => Resources.Load<GameObject>(NukePath);
        public Task<GameObject> LoadNukeFx() => Addressables.LoadAssetAsync<GameObject>("NukeFx").Task;
    }
}