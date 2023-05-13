using System;
using DefaultNamespace.Skins;

namespace Services
{
    public interface IPropertyService
    {
        event Action<IAcquirable> PropertyOwned;
        bool IsOwned(IAcquirable acquirable);
        void Own(IAcquirable acquirable);
    }
}