using MathUtility;

namespace Upgrading.Services.CostEstimation
{
    public class UpgradeCostService : IUpgradeCostService
    {
        public int Calculate(IEstimated unit)
        {
            return (int) Functions.Polynomial(
                x: unit.UpgradedLevel * unit.Tier, 
                n: CostEstimationConstants.UpgradeCostGrowMultiplier,
                add: CostEstimationConstants.BaseCostAmount);
        }
    }
}