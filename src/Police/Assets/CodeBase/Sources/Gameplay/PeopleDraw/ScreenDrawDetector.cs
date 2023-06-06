using System;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PeopleDraw
{
    public class ScreenDrawDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private InputService _input;

        private bool _isIn;

        private void FixedUpdate()
        {
            if (_isIn && Input.GetMouseButton(0))
            {
                _input.DragInProgress();
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData) => _isIn = true;

        public void OnPointerExit(PointerEventData eventData) => _isIn = false;
    }
}