using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fx
{
    public class ColorFlickeringAnimation : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _texts;
        [SerializeField] private Image[] _images;
        [SerializeField] private Color[] _colors;
        [SerializeField] private float _colorTime;

        private void OnEnable() => StartCoroutine(Flickering());

        private IEnumerator Flickering()
        {
            while (true)
            {
                foreach (Color current in _colors)
                {
                    ChangeColor(current);
                    yield return new WaitForSeconds(_colorTime);
                }
            }
        }

        private void ChangeColor(Color current)
        {
            foreach (Image image in _images)
            {
                image.DOColor(current, _colorTime);
            }

            foreach (TMP_Text tmpText in _texts)
            {
                tmpText.DOColor(current, _colorTime);
            }
        }
    }
}