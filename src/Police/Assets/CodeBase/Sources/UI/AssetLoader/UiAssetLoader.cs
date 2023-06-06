using Barracks;
using UnityEngine;

namespace UI.AssetLoader
{
    public class UiAssetLoader
    {
        public BarracksUnitElement LoadBarracksElement()
            => Resources.Load<BarracksUnitElement>("Prefabs/UI/BarracksUnitEntry");
    }
}