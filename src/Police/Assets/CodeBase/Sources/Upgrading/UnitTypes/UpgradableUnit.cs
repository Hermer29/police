using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Services.UseService;
using SpecialPlatforms;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace Upgrading.UnitTypes
{
    public abstract class UpgradableUnit : ScriptableObject, ISavableData, IEstimated, ITypedUnit<UnitType>
    {
        [field: SerializeField] public string Guid { get; private set; }
        [field: SerializeField] public bool OwnedByDefault { get; private set; }
        [field: SerializeField] public int Tier { get; private set; }
        [field: SerializeField] public UnitType Type { get; private set; }
        [field: SerializeField] public bool Purchasable { get; private set; }
        [field: SerializeField] public TableEntryReference LocalizedName { get; private set; }

        [field: NonSerialized] public ReactiveProperty<int> UpgradedLevel { get; private set; } = new(initialValue: 1);
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
            if (IsCanUpgrade() == false)
                return;
            
            UpgradedLevel.Value++;
            OnLevelIncremented();
        }

        protected abstract bool IsCanUpgrade(); // ¯\_(ツ)_/¯

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
            UpgradedLevel = new ReactiveProperty<int>(result.upgradeLevel);
        }

        protected virtual void OnLevelIncremented() { }
    }
}