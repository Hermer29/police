using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class Arrow : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float _normalizedOffsetFromCenter = 0.6f;
        [SerializeField, Min(.1f)] private float _speed = 9999;
        [SerializeField] private CanvasGroup _fading;
        [SerializeField] private RectTransform _rectTransform;
        [Header("Additional target")]
        [SerializeField] private RectTransform _additionalRectTransform;
        [SerializeField, Range(0, 1)] private float _additionalNormalizedOffsetFromCenter = 0.6f;
        
        private Vector2? _targetDirection;
        private Vector2 _previousDirection;
        private Camera _camera;
        private bool _disabled;

        private void Start()
        {
            _fading.alpha = 0;
            _fading.interactable = false;
            _fading.blocksRaycasts = false;
        }

        public void ShowTowards(Vector3 playerPosition, Vector3 targetPosition)
        {
            _camera ??= Camera.main;
            
            _fading.alpha = 1;
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, .4f);
            Vector2 twoDimDirection = -playerPosition.XZ3DDirectionIntoXY2D(targetPosition) * _normalizedOffsetFromCenter;
            twoDimDirection = DetermineArrowRotation(twoDimDirection);
            _previousDirection = _targetDirection ?? twoDimDirection;
            _targetDirection = twoDimDirection;
        }

        private Vector3 CalculatePosition(Vector3 position)
        {
            var prevPosition = _rectTransform.position;
            var viewport = Camera.main.WorldToViewportPoint(position)
                .Clamp(new Vector3(.2f, .2f), new Vector3(.8f, .8f));
            return prevPosition.SetY(Camera.main.ViewportToScreenPoint(viewport).y);
        }

        private static Vector2 DetermineArrowRotation(Vector2 twoDimDirection)
        {
            float dot = Vector3.Dot(Vector2.left, twoDimDirection);
            const float sensitivity = 0.3f;
            if (dot is < sensitivity and > -sensitivity)
            {
                return Vector2.up;
            }

            bool leansRight = dot < -0.3f;
            return leansRight ? Vector2.right : Vector2.left;
        }

        private IEnumerator WaitALittle(Vector3 from, Vector3 to)
        {
            ShowTowards(from.SetY(0), to.SetY(0));
            SlerpTowardsTarget(Time.deltaTime);
            var screenPoint = CalculatePosition(to);
            _rectTransform.position = screenPoint;
            _additionalRectTransform.position = screenPoint + ((Vector3) _targetDirection.Value) * (100 * -1);
            yield return new WaitForSeconds(3);
            Hide();
        }

        public Vector3 CalculateVectorTowardsCenter(Vector3 screenPoint)
        {
            return (_camera.ViewportToScreenPoint(new Vector2(.5f, .5f)) - screenPoint).normalized;
        }

        public void ShowArrowTowardsTarget(Vector3 from, Vector3 to) 
            => StartCoroutine(WaitALittle(from, to));

        private void SlerpTowardsTarget(float deltaTime)
        {
            var direction = (Vector2)Vector3.Slerp(_previousDirection, _targetDirection.Value, deltaTime * _speed);
            Vector2 screenPoint = DirectionToScreenPoint(direction, _normalizedOffsetFromCenter);
            Vector2 additionalScreenPoint = DirectionToScreenPoint(direction, _additionalNormalizedOffsetFromCenter);
            _rectTransform.position = screenPoint;
            if(_additionalRectTransform != null)
                _additionalRectTransform.position = additionalScreenPoint;
            _rectTransform.rotation = CalculateRotationTowards(direction);
        }

        private static Quaternion CalculateRotationTowards(Vector2 direction)
        {
            float angle = Vector2.SignedAngle(Vector3.right, direction);
            return Quaternion.Euler(0, 0, angle);
        }

        public void Hide()
        {
            _fading.alpha = 0;
            _targetDirection = null;
        }

        public void Disable()
        {
            _fading.alpha = 0;
            _disabled = true;
        }

        public void Enable()
        {
            _disabled = false;
        }

        private Vector2 ToScreenPosition(Vector2 viewport)
        {
            return Camera.main.ViewportToScreenPoint(viewport);
        }

        private void OnDrawGizmos()
        {
            void DrawSphereInDirection(Vector2 direction)
            {
                Gizmos.color = Color.red;
                Vector2 screen = DirectionToScreenPoint(direction, _normalizedOffsetFromCenter);
            
                Gizmos.DrawWireSphere(_camera.ScreenPointToRay(screen).GetPoint(5), .5f);
            }

            if (_camera == null)
                return;
        
            DrawSphereInDirection(Vector2.up);
            DrawSphereInDirection(Vector2.down);
            DrawSphereInDirection(Vector2.right);
            DrawSphereInDirection(Vector2.left);
        }

        private Vector2 DirectionToScreenPoint(Vector2 direction, float normalizedOffsetFromCenter)
        {
            return ToScreenPosition(direction.NormalizedDirectionToViewport(normalizedOffsetFromCenter));
        }
    }

    public static class Vector2Extensions
    {
        public static Vector2 XZ3DDirectionIntoXY2D(this Vector3 worldA, Vector3 worldB)
        {
            worldA.y = 0;
            worldB.y = 0;
            Vector3 direction = worldB - worldA;
            return new Vector2(direction.x, direction.z);
        }
    
        public static Vector2 NormalizedDirectionToViewport(this Vector2 direction, float scale)
        {
            Vector2 normalized = direction.normalized;
            float x = (normalized.x * scale) * 0.5f + 0.5f;
            float y = (normalized.y * scale) * 0.5f + 0.5f;
            return new Vector2(x, y);
        }
    }
}