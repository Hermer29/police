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
        private bool _holding;
        private Vector3? _startPoint;

        private void Update()
        {
            if (_isIn == false)
            {
                if (_holding)
                {
                    _holding = false;
                    _startPoint = null;
                    _input.OnDragEnded();
                }
                return;
            }
            
            if (JustClicked())
            {
                Debug.Log($"{nameof(ScreenDrawDetector)} Click started");
                _startPoint = Input.mousePosition;
                _input.OnDrawnAtPoint();
            }

            if (Input.GetMouseButton(0) && _startPoint != null && _holding == false)
            {
                Debug.Log($"{nameof(ScreenDrawDetector)} Drag started");
                _holding = true;
                _input.OnDragStarted();
            }

            if (Input.GetMouseButton(0) && _holding)
            {
                _input.OnDragInProgress();
            }

            if (Input.GetMouseButton(0) == false && _holding)
            {
                Debug.Log($"{nameof(ScreenDrawDetector)} Drag ended");
                _startPoint = null;
                _holding = false;
                _input.OnDragEnded();
            }
        }

        private bool JustClicked()
        {
            return Input.GetMouseButton(0) && _startPoint == null;
        }

        public void OnPointerEnter(PointerEventData eventData) => _isIn = true;

        public void OnPointerExit(PointerEventData eventData) => _isIn = false;
    }
}