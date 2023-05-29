using System;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using Upgrading;

namespace Interface
{
    public class CurrentLevelBarracksViewer : MonoBehaviour
    {
        [Header("Configs")] 
        [SerializeField] private ProgressSegment _prefab;
        [SerializeField] private Transform _parentForSegments;
        [SerializeField] private ProgressBlock _progress;
        [SerializeField] private TMP_Text _lastLevelLabel;
        [SerializeField] private GameObject _lastLevelObject;
        
        [Header("Visual testing")] 
        [SerializeField] private bool _testing;
        [SerializeField] private int _currentLevel;
        [SerializeField] private int _maximumLevel;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if(_testing)
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    SetCurrentAndLastLevel(_currentLevel, _maximumLevel);
                }; 
#endif
        }

        public void SetCurrentAndLastLevel(int currentLevel, int maximumLevel)
        {
            if (currentLevel > maximumLevel)
                throw new ArgumentOutOfRangeException();
            
            _progress.ShowLevel(currentLevel);
            HandleSegments(currentLevel, maximumLevel);
        }

        private void HandleSegments(int currentLevel, int maximumLevel)
        {
            DeletePreviousSegments();
            var maximumSuperLevel = LevelsPartCalculator.CalculateSuperLevel(maximumLevel);
            if(_testing)
                Debug.Log($"{nameof(maximumSuperLevel)} = {maximumSuperLevel}");
            if (LevelsPartCalculator.CalculateNextSuperLevel(currentLevel) == maximumSuperLevel)
            {
                DeletePreviousSegments();
                _lastLevelObject.SetActive(false);
                return;
            }
            _lastLevelObject.SetActive(true);
            _lastLevelLabel.text = maximumSuperLevel.ToString();
            int howMuchSegmentsToShow = maximumLevel - currentLevel - 3;
            if(_testing)
                Debug.Log($"{nameof(howMuchSegmentsToShow)} = {howMuchSegmentsToShow}");
            
            DeletePreviousSegments();
            for (int i = 0; i < howMuchSegmentsToShow; i++)
                AppendUnlitSegment();
        }

        private void AppendUnlitSegment() 
            => Instantiate(_prefab, _parentForSegments);

        private void DeletePreviousSegments()
        {
            foreach (Transform segment in _parentForSegments.Cast<Transform>()) 
                DestroyImmediate(segment.gameObject);
        }
    }
}