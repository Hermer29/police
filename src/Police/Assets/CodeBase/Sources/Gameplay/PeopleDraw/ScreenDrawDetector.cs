using Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PeopleDraw
{
    public class ScreenDrawDetector : MonoBehaviour, IDragHandler
    {
        [SerializeField] private InputService _input;
        
        public void OnDrag(PointerEventData eventData)
        {
            _input.DragInProgress();
        }
    }
}