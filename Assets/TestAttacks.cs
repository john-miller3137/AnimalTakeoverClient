using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttacks : MonoBehaviour
{
    public Vector3 targetPos;           // The position the object will move towards
    public float moveSpeed = 5f;        // The speed at which the object moves
    public float maxAmplitude = 5f;     // The maximum amplitude of the sine wave
    public float amplitudeDecay = 0.5f; // The rate at which the amplitude decreases over time
    public float waveOffset = 2f;       // The offset for each additional sine wave

    private float startTime;            // The time at which the movement started
    private float maxDistance;          // The maximum distance from the target position
    private float maxTime;              // The time it will take for the object to reach the target position
    private Vector3 startPos;           // The starting position of the object

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        startPos = transform.position;
        maxDistance = (targetPos - startPos).magnitude;
        maxTime = maxDistance / moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the time elapsed since the movement started
        float fracJourney = (Time.time - startTime) / maxTime;

        // Calculate the amplitude of the sine wave based on the distance to the target position
        float amplitude = Mathf.Lerp(maxAmplitude, 0f, fracJourney);

        // Calculate the position of the object along the sine wave
        float t = Mathf.Lerp(0f, Mathf.PI * 2f, fracJourney) + (waveOffset * Mathf.PI);
        float y = Mathf.Sin(t) * amplitude * (Mathf.Abs(targetPos.y - startPos.y) / maxDistance);
        float x = Mathf.Cos(t) * amplitude * (Mathf.Abs(targetPos.x - startPos.x) / maxDistance);
        Vector3 newPos = Vector3.Lerp(startPos, targetPos, fracJourney);
        newPos.y += y;
        newPos.x += x;

        // Move the object to the new position
        transform.position = newPos;

        // Destroy the object if it has reached the target position
        if (fracJourney >= 1f)
        {
            Destroy(gameObject);
        }
    }
}