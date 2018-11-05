using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Processors;

namespace Anathema.Input
{
    /// <summary>
    /// Processes a Vector2 to apply deadzoning according to the magnitude of the vector (rather
    /// than just clamping individual axes). Normalizes to the min/max range.
    /// </summary>
    public class DeadzoneFloatProcessor : IInputControlProcessor<float>
    {
        /// <summary>
        /// Value at which the lower bound deadzone starts.
        /// </summary>
        /// <remarks>
        /// Values in the input at or below min will get dropped and values
        /// will be scaled to the range between min and max.
        /// </remarks>
        public float min;
        public float max;

        public float minOrDefault
        {
            get { return min == 0.0f ? InputConfiguration.DeadzoneMin : min; }
        }

        public float maxOrDefault
        {
            get { return max == 0.0f ? InputConfiguration.DeadzoneMax : max; }
        }

        public float Process(float value, InputControl control)
        {
            Debug.Log("process");
            return GetDeadZoneAdjustedValue(value);
        }

        private float GetDeadZoneAdjustedValue(float value)
        {
            var min = minOrDefault;
            var max = maxOrDefault;

            var absValue = Mathf.Abs(value);
            if (absValue < min)
                return 0;
            if (absValue > max)
                return Mathf.Sign(value);

            return Mathf.Sign(value) * ((absValue - min) / (max - min));
        }
    }
}