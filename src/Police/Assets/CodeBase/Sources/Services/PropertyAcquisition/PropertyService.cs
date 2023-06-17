using System;
using Services.PrefsService;

namespace Services.PropertyAcquisition
{
    public class PropertyService
    {
        private readonly IPrefsService _prefsService;

        public PropertyService(IPrefsService prefsService)
        {
            _prefsService = prefsService;
        }
        
        public void Own(Property property)
        {
            Increment(property);
        }

        public void OwnMultiple(Property property, int amount)
        {
            Increment(property, amount);
        }

        public int GetQuantity(Property property) 
            => _prefsService.GetInt(ConstructKey(property));

        public bool TryConsume(Property property)
        {
            if (GetAmount(property) == 0)
            {
                return false;
            }

            Decrement(property);
            return true;
        }

        public void Consume(Property property)
        {
            if (TryConsume(property) == false)
                throw new InvalidOperationException();
        }

        private void Increment(Property property, int amount = 1)
        {
            _prefsService.SetInt(ConstructKey(property), _prefsService.GetInt(ConstructKey(property)) + amount);
        }

        private void Decrement(Property property) 
            => _prefsService.SetInt(ConstructKey(property), _prefsService.GetInt(ConstructKey(property)) - 1);

        private int GetAmount(Property property) => _prefsService.GetInt(ConstructKey(property));

        private string ConstructKey(Property property) 
            => $"Property_{property}";
    }
}