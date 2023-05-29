using System;
using Upgrading.UnitTypes;

namespace Services.UseService
{
    public interface ITypedUnit<TEnum> where TEnum: Enum
    {
        TEnum Type { get; }
        public string Guid {get;}
    }
}