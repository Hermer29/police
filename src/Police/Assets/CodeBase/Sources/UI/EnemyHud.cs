using System.Collections;
using UnityEngine;

public class EnemyHud : MonoBehaviour
{
    private Vector3 _target;
    public CanvasGroup CanvasGroup;

    private const float NormalizedDistanceToShow = .2f;
    private static float HorizontalScreenDistanceToShow = Screen.width * NormalizedDistanceToShow;
    private static float VerticalScreenDistanceToShow = Screen.height * NormalizedDistanceToShow;
    private const float HudFollowingSpeed = 15;

    private const float HudZTargetOffset = 5;

    private const float ScreenDistanceBetweenTextsToHideOne = 200 * 200;

    private Vector2 _screenPoint;

    private Vector2 _actualScreenPosition;

    private float _distanceToCenter;

    private bool _hidden = true;

    private bool _visible;

    public void ShowSpawnPoint(Vector3 spawnPointPosition)
    {
        _target = spawnPointPosition;
        StartCoroutine(WaitALittle(spawnPointPosition));
    }

    private IEnumerator WaitALittle(Vector3 point)
    {
        ShowAtPoint(point);
        yield return new WaitForSeconds(1);
        Hide();
    }

    private void ShowAtPoint(Vector3 point)
    {
        CanvasGroup.alpha = 1;
        point.z += HudZTargetOffset; 
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(point);
        _screenPoint = screenPoint;
        _distanceToCenter = (screenPoint - Vector2.zero).sqrMagnitude;
        var clamped = ClampScreenPoint(screenPoint);
        _actualScreenPosition = clamped;
        transform.position = clamped;
    }

    private void Hide()
    {
        CanvasGroup.alpha = 0;
    }

    private Vector3 ClampScreenPoint(Vector3 screenPoint)
    {
        var viewportBoundsXMin = .25f;
        var viewportBoundsXMax = .8f;
        var viewportBoundsYMin = .1f;
        var viewportBoundsYMax = .85f;
        var screenBoundsXMin = Screen.width * viewportBoundsXMin;
        var screenBoundsXMax = Screen.width * viewportBoundsXMax;
        var screenBoundsYMin = Screen.height * viewportBoundsYMin;
        var screenBoundsYMax = Screen.height * viewportBoundsYMax;
        var x = Mathf.Clamp(screenPoint.x, screenBoundsXMin, screenBoundsXMax);
        var y = Mathf.Clamp(screenPoint.y, screenBoundsYMin, screenBoundsYMax);
        var clamped = new Vector3(x, y);
        return clamped;
    }
}