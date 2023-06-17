using System;
using Services.PropertyAcquisition;

namespace Services.SuperPowersService
{
    public enum SuperPower
    {
        SuperUnit,
        Nuke
    }

    public static class SuperPowerExtensions
    {
        public static Property AsProperty(this SuperPower superPower)
        {
            return superPower switch
            {
                SuperPower.Nuke => Property.Nuke,
                SuperPower.SuperUnit => Property.SuperUnit,
                _ => throw new ArgumentOutOfRangeException(nameof(superPower), superPower, null)
            };
        }
    }
    
}