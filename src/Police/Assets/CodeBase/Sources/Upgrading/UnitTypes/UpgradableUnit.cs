using System.Collections.Generic;
using Newtonsoft.Json;
using Services.UseService;
using SpecialPlatforms;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Upgrading.UnitTypes
{
    public abstract class UpgradableUnit : ScriptableObject, ISavableData, IEstimated, ITypedUnit<UnitType>
    {
        [field: SerializeField] public string Guid { get; private set; }
        [field: SerializeField] public bool OwnedByDefault { get; private set; }
        [field: SerializeField] public int Tier { get; private set; }
        [field: SerializeField] public UnitType Type { get; private set; }
        [field: SerializeField] public bool Purchasable { get; private set; }
        
        public ReactiveProperty<int> UpgradedLevel { get; } = new(initialValue: 1);
        int IEstimated.UpgradedLevel => UpgradedLevel.Value;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(Guid))
            {
                Guid = System.Guid.NewGuid().ToString();
                EditorUtility.SetDirty(this);
            }
#endif
        }

        public void Upgrade()
        {
            UpgradedLevel.Value++;
            OnLevelIncremented();
        }

        string ISavableData.GetData()
        {
            var data = new Dictionary<string, string>()
            {
                {"upgradeLevel", UpgradedLevel.ToString()}
            };
            return JsonConvert.SerializeObject(data);
        }

        string ISavableData.Key() => Guid;

        void ISavableData.Populate(string data)
        {
            var result = JsonConvert.DeserializeAnonymousType(data,
                new { upgradeLevel = 1 });
            UpgradedLevel.Value = result.upgradeLevel;
        }

        public static bool operator ==(UpgradableUnit a, UpgradableUnit b) 
            => a.Guid == b.Guid;

        public static bool operator !=(UpgradableUnit a, UpgradableUnit b) 
            => a.Guid != b.Guid;

        protected virtual void OnLevelIncremented() { }
    }
}