using System.Collections.Generic;
using Upgrading.UnitTypes;

namespace Upgrading.Services
{
    public interface ISelectService
    {
        void Define(IEnumerable<UpgradableUnit> units);
        void Reselect(UpgradableUnit unit);
    }
}