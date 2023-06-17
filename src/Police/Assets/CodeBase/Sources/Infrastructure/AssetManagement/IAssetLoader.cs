using System.Threading.Tasks;
using Gameplay.Levels.UI.CrossLevelUi;
using Tutorial;
using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public interface IAssetLoader
    {
        CrossLevelUi LoadCrossLevelUI();
        TutorialEngine LoadTutorial();
        GameObject LoadNuke();
        Task<GameObject> LoadNukeFx();
    }
}