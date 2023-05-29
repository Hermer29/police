using System;
using UI.AssetLoader;
using Barracks;
using UnityEngine;
using Upgrading.UnitTypes;
using Object = UnityEngine.Object;

namespace UI.Factory
{
    public class UiElementsFactory
    {
        private readonly UiAssetLoader _assetLoader;

        public UiElementsFactory(UiAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public event Action<PartialUpgradableUnit> OnOpenedWindow;

        public BarracksUnitElement CreateBarracksElement(Transform parent)
        {
            BarracksUnitElement unitElement = Object.Instantiate(_assetLoader.LoadBarracksElement(), parent);
            unitElement.OpenWindowRequested += OnOpenedWindow;
            return unitElement;
        }
    }
}