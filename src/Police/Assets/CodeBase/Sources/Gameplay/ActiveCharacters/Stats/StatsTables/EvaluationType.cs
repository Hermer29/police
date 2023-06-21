using System;
using MathUtility;

namespace DefaultNamespace.Gameplay.ActiveCharacters.Stats.StatsTables
{
    public enum EvaluationType
    {
        Linear,
        Polynomial
    }

    public static class EvaluationTypeExtensions
    {
        public static float CalculateFor(this EvaluationType evaluationType, float grow, int level)
        {
            return evaluationType switch
            {
                EvaluationType.Linear => grow * level,
                EvaluationType.Polynomial => Functions.Polynomial(level, grow),
                _ => throw new ArgumentOutOfRangeException(nameof(evaluationType))
            };
        }

        public static float EvaluateFor(this EvaluationType evaluationType, float grow, int level, float initial)
        {
            var calculated = CalculateFor(evaluationType, grow, level);
            return calculated + initial;
        }
    }
}