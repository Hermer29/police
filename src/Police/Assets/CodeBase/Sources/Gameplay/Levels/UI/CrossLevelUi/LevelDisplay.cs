using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Levels.UI.CrossLevelUi
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private Image[] _segments;
        [SerializeField] private Color _completed;
        [SerializeField] private Color _current;
        [SerializeField] private Color _others;
        
        public void ShowLocalLevel(int level)
        {
            for (var i = 0; i < _segments.Length; i++)
            {
                if (i < level - 1)
                {
                    _segments[i].color = _completed;
                }
                else if (i == level - 1)
                {
                    _segments[i].color = _current;
                }
                else
                {
                    _segments[i].color = _others;
                }
            }
        }
    }
}