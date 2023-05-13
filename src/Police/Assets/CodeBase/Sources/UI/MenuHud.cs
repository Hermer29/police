using Shop;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class MenuHud : MonoBehaviour
    {
        private ShopWindow _shopWindow;
        
        [SerializeField] private Button _shopWindowOpen;

        [Inject]
        public void Construct(ShopWindow shopWindow)
        {
            _shopWindow = shopWindow;
            
            _shopWindowOpen.onClick.AddListener(_shopWindow.Open);
        }
    }
}