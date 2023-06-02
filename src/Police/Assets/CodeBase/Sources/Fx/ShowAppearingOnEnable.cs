using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Fx
{
    public class ShowAppearingOnEnable : MonoBehaviour
    {
        private void OnEnable()
        {
            ShowAppearing();
        }

        [Button]
        private void ShowAppearing()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, .4f);
        }

        private void OnDisable() => transform.DOKill();
    }
}