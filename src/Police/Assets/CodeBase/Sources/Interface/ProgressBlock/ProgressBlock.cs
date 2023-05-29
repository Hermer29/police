using System;
using ModestTree;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Upgrading;

namespace Interface
{
    public class ProgressBlock : MonoBehaviour
    {
        [SerializeField] private ProgressSegment[] _progressImages;
        [SerializeField] private TMP_Text _previousSuperLevel;
        [SerializeField] private TMP_Text _nextSuperLevel;

        [Header("Visual testing")] 
        [SerializeField] private bool _currentLevelTesting;
        [SerializeField] private int _currentLevel = 1;
        [SerializeField] private bool _scaleTesting;
        [SerializeField] private int _scaleValue = 0;

        private void OnValidate()
        {
            if(_currentLevelTesting)
                ShowLevel(_currentLevel);
            if(_scaleTesting)
                ShowScale(_scaleValue);
        }

        public void ShowLevel(int level)
        {
            Assert.That(level > 0);

            int currentSuperLevel = CalculateSuperLevel(level);
            
            _previousSuperLevel.text = (currentSuperLevel).ToString();
            _nextSuperLevel.text = (currentSuperLevel + 1).ToString();
          
            int litBars = (level - 1) % UpgradeUnitsConstants.LevelsToUpgradePart;
            ShowScale(litBars);
        }

        private static int CalculateSuperLevel(int level) 
            => LevelsPartCalculator.CalculateSuperLevel(level);

        private void ShowScale(int litCount)
        {
            if (litCount is > 3 or < 0)
                throw new ArgumentOutOfRangeException(nameof(litCount));
            
            for (var i = 0; i < _progressImages.Length; i++)
            {
                var current = _progressImages[i];
                if (i < litCount)
                {
                    current.Light();
                    continue;
                }
                current.Extinguish();
            }
        }
    }
}