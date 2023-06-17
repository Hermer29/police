using System;
using DG.Tweening;

namespace Helpers
{
    public static class DoTweenExtensions
    {
        public static Sequence Then(this Tween tween, Tween continuation)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(tween);
            sequence.Append(continuation);
            return sequence;
        }

        public static Sequence Then(this Sequence sequence, Tween continuation)
        {
            sequence.Append(continuation);
            return sequence;
        }

        public static Sequence ThenDelay(this Sequence sequence, float delay)
        {
            sequence.AppendInterval(delay);
            return sequence;
        }

        public static Sequence ThenDelay(this Tween tween, float delay)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(tween);
            sequence.AppendInterval(delay);
            return sequence;
        }

        public static Sequence Then(this Tween tween, Action action)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(tween);
            sequence.AppendCallback(() => action());
            return sequence;
        }
        
        public static Sequence Then(this Sequence sequence, Action action)
        {
            sequence.AppendCallback(() => action());
            return sequence;
        }
    }
}