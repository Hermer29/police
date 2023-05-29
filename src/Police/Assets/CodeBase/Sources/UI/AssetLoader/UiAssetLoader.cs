using Barracks;
using UnityEngine;

namespace UI.AssetLoader
{
    public class UiAssetLoader
    {
        public UiAssetLoader()
        {
            
        }
        
        public BarracksUnitElement LoadBarracksElement()
        {
            return Resources.Load<BarracksUnitElement>("Prefabs/UI/BarracksUnitElement");
        }
    }
}