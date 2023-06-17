using Gameplay.Levels.UI.CrossLevelUi;
using Tutorial;
using UnityEngine;

namespace Infrastructure
{
    public interface IGameFactory
    {
        CrossLevelUi CreateCrossLevelUI();
        TutorialEngine CreateTutorial();
        GameObject CreateNuke();
        void CreateNukeFx(Vector3 at);
    }
}