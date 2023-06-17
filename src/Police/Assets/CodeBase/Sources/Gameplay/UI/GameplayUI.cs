using PeopleDraw.EnergyConsumption;
using Selection;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [field: SerializeField] public EnergyUi EnergyUi { get; private set; }
        [field: SerializeField] public SelectorUi SelectorUi { get; private set; }
        [field: SerializeField] public Button Replay { get; private set; }

        [SerializeField] private Image[] Images;
        
        public void Hide() => gameObject.SetActive(false);
        private void Show() => gameObject.SetActive(true);

        public void SetIcons(params Sprite[] sprites)
        {
            for (var i = 0; i < sprites.Length; i++)
            {
                Images[i].sprite = sprites[i];
            }
            Show();
        }
    }
}