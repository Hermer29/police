﻿using PeopleDraw.EnergyConsumption;
using Selection;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [field: SerializeField] public EnergyUi EnergyUi { get; private set; }
        [field: SerializeField] public SelectorUi SelectorUi { get; private set; }
        [field: SerializeField] public Button Replay { get; private set; }
        public void Hide() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);
    }
}