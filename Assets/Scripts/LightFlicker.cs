
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Scripts
{
    

    public class LightFlicker : MonoBehaviour
    {
        public Light2D light2D;
        private float minIntensity = 0.4f;
        private float maxIntensity = 0.6f;

        private float targetIntensity;
        private float currentIntensity;
        private float flickerTime;

        private void Start()
        {
            // Get the Light2D component attached to the GameObject
            //light2D = GetComponent<Light2D>();

            // Set the initial intensity to the minimum value
            currentIntensity = minIntensity;

            // Start the first flicker
            StartFlicker();
        }

        private void Update()
        {
            // Update the flickering effect
            Flicker();
        }

        private void Flicker()
        {
            // Check if it's time to change the intensity
            if (Time.time >= flickerTime)
            {
                // Set a random intensity within the specified range
                targetIntensity = Random.Range(minIntensity, maxIntensity);

                // Set the new time for the next flicker
                flickerTime = Time.time + Random.Range(0.1f, 0.5f);

                // Reset the current intensity
                currentIntensity = minIntensity;
            }

            // Smoothly change the intensity towards the target intensity
            currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * 5f);

            // Apply the new intensity to the Light2D component
            light2D.intensity = currentIntensity;
        }

        private void StartFlicker()
        {
            // Calculate the seed based on the current x and y positions
            int seed = Mathf.RoundToInt(transform.position.x + transform.position.y);

            // Initialize the random number generator with the calculated seed
            Random.InitState(seed);

            // Set the initial time for the first flicker
            flickerTime = Time.time + Random.Range(0.1f, 0.5f);
        }
    }
}