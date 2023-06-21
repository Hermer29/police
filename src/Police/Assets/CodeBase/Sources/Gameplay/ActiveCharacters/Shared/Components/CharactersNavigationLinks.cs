using DefaultNamespace.Gameplay;
using DefaultNamespace.Gameplay.ActiveCharacters.Shared;
using Enemies.Components;
using Gameplay.ActiveCharacters.Shared.Components;
using UnityEngine;
using Upgrading.UnitTypes;

namespace ActiveCharacters.Shared.Components
{
    public class CharactersNavigationLinks : MonoBehaviour
    {
        public Attackable Attackable;
        public RunToWin RunToWin;
        public Attacker Attacker;
        public RangeApplier RangeChanger;

        public void SetBalance(AlliedUnitsBalanceDto balanceDto)
        {
            Attackable?.SetHealth(balanceDto.Health);
            Attacker?.SetStats(balanceDto.Damage);
            RangeChanger?.SetRange(balanceDto.Range);
        }

        public void SetBalance(UnitBalanceDto balanceDto)
        {
            Attackable?.SetHealth(balanceDto.Health);
            Attacker?.SetStats(balanceDto.Damage);
        }
    }
}