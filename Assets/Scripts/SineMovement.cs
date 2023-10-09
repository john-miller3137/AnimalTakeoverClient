using UnityEngine;

namespace Scripts
{
    public class SineMovement : MonoBehaviour
    {
        public float amplitudeX = 1f; // The amplitude of the sine wave in the X direction
        public float frequencyX = 1f; // The frequency of the sine wave in the X direction
        public float amplitudeY = 1f; // The amplitude of the sine wave in the Y direction
        public float frequencyY = 1f; // The frequency of the sine wave in the Y direction
        public float duration = 2f; // The duration of the movement
        private Vector2 initialPosition; // The initial position of the object
        private float elapsedTime = 0f; // The elapsed time since the movement started

        private void Start()
        {
            initialPosition = transform.position;
        }

        private void Update()
        {
            // Calculate the progress based on the elapsed time and duration
            float progress = Mathf.Clamp01(elapsedTime / duration);

            // Apply in-out cubic easing to the progress
            float easedProgress = Easing.InOutCubic(progress);

            // Calculate the new position based on the sine waves and eased progress
            float newX = initialPosition.x + Mathf.Sin(frequencyX * 2f * Mathf.PI * easedProgress) * amplitudeX;
            float newY = initialPosition.y + Mathf.Sin(frequencyY * 2f * Mathf.PI * easedProgress) * amplitudeY;

            // Move the object to the new position
            transform.position = new Vector3(newX, newY, transform.position.z);

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Reset the elapsed time if it exceeds the duration
            if (elapsedTime >= duration)
            {
                elapsedTime = 0f;
            }
        }
    }
}