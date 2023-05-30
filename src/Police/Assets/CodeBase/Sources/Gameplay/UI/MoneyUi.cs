using Services.MoneyService;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class MoneyUi : MonoBehaviour
    {
        private IMoneyService _moneyService;
        
        [SerializeField] private TMP_Text _money;

        [Inject]
        public void Construct(IMoneyService moneyService)
        {
            _moneyService = moneyService;

            AssignMoneyAmount(_moneyService.Amount);

            _moneyService.Spent += delta => MoneyChanged();
            _moneyService.Gained += delta => MoneyChanged();
        }

        public void Show()
        {
            Debug.Log($"{nameof(MoneyUi)}.{nameof(Show)} called");
            
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            Debug.Log($"{nameof(MoneyUi)}.{nameof(Hide)} called");

            gameObject.SetActive(false);
        }

        private void AssignMoneyAmount(int moneyServiceAmount) 
            => _money.text = moneyServiceAmount.ToString();

        private void MoneyChanged() 
            => AssignMoneyAmount(_moneyService.Amount);
    }
}