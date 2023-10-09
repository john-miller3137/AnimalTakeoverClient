using UnityEngine;

namespace Scripts
{
    public class GoTextMovement : MonoBehaviour
    {
        public float finalY = 10f; // The final Y position to reach
        public float duration = 1f; // The duration of the movement
        private float initialY; // The initial Y position of the object
        private float elapsedTime = 0f; // The elapsed time since the movement started
        private bool isMoving = false; // Flag to track if the object is currently moving

        private bool isReverseEasing = true; // Flag to track if reverse easing is active
        private float reverseDuration = 0.2f; // The duration of the reverse easing
        private float reverseElapsedTime = 0f; // The elapsed time for the reverse easing

        private void Start()
        {
            initialY = transform.position.y;
            StartMovement();
        }

        private void Update()
        {
            if (isMoving)
            {
                if (isReverseEasing)
                {
                    // Apply reverse easing
                    float reverseProgress = Mathf.Clamp01(reverseElapsedTime / reverseDuration);
                    float easedReverseProgress = Easing.InOutCubic(reverseProgress);
                    float newY = Mathf.Lerp(finalY, initialY, easedReverseProgress);
                    transform.position = new Vector3(transform.position.x, newY, transform.position.z);

                    reverseElapsedTime += Time.deltaTime;

                    if (reverseElapsedTime >= reverseDuration)
                    {
                        isReverseEasing = false;
                        elapsedTime = 0f;
                    }
                }
                else
                {
                    // Apply normal easing
                    float progress = Mathf.Clamp01(elapsedTime / duration);
                    float easedProgress = Easing.InOutCubic(progress);
                    float newY = Mathf.Lerp(initialY, finalY, easedProgress);
                    transform.position = new Vector3(transform.position.x, newY, transform.position.z);

                    elapsedTime += Time.deltaTime;

                    if (elapsedTime >= duration)
                    {
                        isMoving = false;
                    }
                }
            }
        }

        public void StartMovement()
        {
            isMoving = true;
            isReverseEasing = true;
            elapsedTime = 0f;
            reverseElapsedTime = 0f;
        }
    }
}