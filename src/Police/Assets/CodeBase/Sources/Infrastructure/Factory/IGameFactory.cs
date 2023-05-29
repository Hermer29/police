using Gameplay.Levels.UI.CrossLevelUi;
using Tutorial;

namespace Infrastructure
{
    public interface IGameFactory
    {
        CrossLevelUi CreateCrossLevelUI();
        TutorialEngine CreateTutorial();
    }
}