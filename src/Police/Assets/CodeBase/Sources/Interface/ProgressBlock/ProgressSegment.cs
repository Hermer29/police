using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class ProgressSegment : MonoBehaviour
    {
        private Image _relatedRect;
        
        [SerializeField] private Color _enabledScaleColor;
        [SerializeField] private Color _disabledScaleColor;

        private void Initialize()
        {
            if (_relatedRect == null)
                _relatedRect = GetComponent<Image>();
        }

        public void Light()
        {
            Initialize();
            _relatedRect.color = _enabledScaleColor;
        }

        public void Extinguish()
        {
            Initialize();
            _relatedRect.color = _disabledScaleColor;
        }
    }
}