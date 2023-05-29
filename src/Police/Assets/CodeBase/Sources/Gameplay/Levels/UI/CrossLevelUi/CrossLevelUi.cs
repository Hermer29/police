using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Upgrading.UI.CrossLevels;

namespace Gameplay.Levels.UI.CrossLevelUi
{
    public class CrossLevelUi : MonoBehaviour
    {
        [SerializeField] private CrossLevelUpgradePresenter _upgrades;

        [field: SerializeField] public Button PlayButton { get; private set; }
        
        private void Validate()
        {
            Assert.IsNotNull(PlayButton);
        }
        
        private void Start()
        {
            Validate();
        }

        public void Initialize()
        {
            _upgrades.LoadListWithData();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _upgrades.Show();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _upgrades.Hide();
        }
    }
}