using UnityEngine;

namespace Scripts
{
    public static class Easing
    {
        public static float InOutCubic(float t)
        {
            return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
        }
        public static float Scale1Quick(float t)
        {
            return 1f - Mathf.Pow(1f - t, 4f);
        }
        public static float OutCubic(float t)
        {
            return 1f - Mathf.Pow(1f - t, 3f);
        }
        
        public static float Anticipate(float t)
        {
            float s = 1.70158f;  // Anticipate constant (adjust as desired)
            return t * t * ((s + 1f) * t - s);
        }

    }
}