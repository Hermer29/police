﻿using Services.SuperPowersService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class SuperPowersUi : MonoBehaviour
    {
        [SerializeField] private GameObject _buttonsRoot;
        [SerializeField] private Button _takeSuperUnit;
        [SerializeField] private Button _takeNuke;
        [SerializeField] private Button _close;
        [SerializeField] private GameObject _windowsRoot;
        
        [SerializeField] private SuperPowersWindow _nuke;
        [SerializeField] private SuperPowersWindow _superUnit;

        [Inject]
        public void Construct(SuperPowers powers)
        {
            _takeSuperUnit.onClick.AddListener(_superUnit.Show);
            _takeNuke.onClick.AddListener(_nuke.Show);
            _close.onClick.AddListener(Close);
            _nuke.Construct(powers, this);
            _superUnit.Construct(powers, this);
            gameObject.SetActive(true);
            Close();
        }

        public void ShowWindows()
        {
            _windowsRoot.SetActive(true);
        }

        public void ShowButtons()
        {
            _buttonsRoot.SetActive(true);
        }

        public void HideButtons()
        {
            _buttonsRoot.SetActive(false);
        }

        public void Close()
        {
            _windowsRoot.SetActive(false);
            _nuke.Hide();
            _superUnit.Hide();
        }
    }
}