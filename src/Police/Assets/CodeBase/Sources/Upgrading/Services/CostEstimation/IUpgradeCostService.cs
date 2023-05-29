namespace Upgrading.Services.CostEstimation
{
    public interface IUpgradeCostService
    {
        int Calculate(IEstimated unit);
    }
}