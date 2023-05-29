using System;
using Upgrading.UnitTypes;

namespace Services.UseService
{
    public class UnitChangesListener : IDisposable
    {
        public PartialUpgradableUnit Unit { get; }

        public UnitChangesListener(PartialUpgradableUnit unit)
        {
            Unit = unit;
        }
        
        public void Dispose()
        {
        }
    }
}