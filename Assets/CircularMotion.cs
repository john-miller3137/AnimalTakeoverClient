using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CircularMotion : MonoBehaviour
{
    public GameObject centerPoint;
    //private SpriteRenderer sr;
    public float radius = 1f;  // Radius of the circular motion
    public float speed = 1f;   // Speed of the circular motion

    // public Light2D light2D;    // Reference to the Light2D component
    private List<ParticleSystemRenderer> pSystems;
    private TrailRenderer trailRenderer;
    private float angle;       // Current angle in radians

    private void Start()
    {
        gameObject.transform.SetParent(centerPoint.transform.GetChild(1));
        pSystems = new List<ParticleSystemRenderer>();
        int childCount = gameObject.transform.GetChild(0).childCount;
        for(int i = 0; i < childCount - 1; i++)
        {
            pSystems.Add(gameObject.transform.GetChild(0).GetChild(i).GetComponent<ParticleSystemRenderer>());
        }
        trailRenderer = gameObject.transform.GetChild(0).GetChild(childCount - 1).GetComponent<TrailRenderer>();
        //light2D = gameObject.GetComponent<Light2D>();
        //sr = gameObject.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        // Increment the angle based on the time and speed
        angle += speed * Time.deltaTime;

        // Calculate the new position using sine and cosine functions
        float x = centerPoint.transform.position.x + Mathf.Sin(angle) * radius;
        float y = centerPoint.transform.position.y + Mathf.Cos(angle) * radius;


        // Set the new position of the GameObject
        transform.position = new Vector3(x, y, transform.position.z);

        // Adjust the sorting layers of the Light2D component based on the rotation
        if (y < centerPoint.transform.position.y)
        {
            /*
            FieldInfo fieldInfo = light2D.GetType().GetField("m_ApplyToSortingLayers", BindingFlags.NonPublic | BindingFlags.Instance);
 
            fieldInfo.SetValue(light2D,  new int[] {
                SortingLayer.NameToID("Animals"),
                SortingLayer.NameToID("Background")
            });
            sr.sortingLayerName = "SpiritParticles";

            */
            foreach(ParticleSystemRenderer psr in pSystems)
            {
                psr.sortingLayerName = "Animals";
            }
            
        }
        else
        {
            /*
            FieldInfo fieldInfo = light2D.GetType().GetField("m_ApplyToSortingLayers", BindingFlags.NonPublic | BindingFlags.Instance);
 
            fieldInfo.SetValue(light2D,  new int[] {
                SortingLayer.NameToID("Background")
            }); // Targets only the Background layer
            sr.sortingLayerName = "Particles";
            */
            foreach (ParticleSystemRenderer psr in pSystems)
            {
                psr.sortingLayerName = "Background";
            }
        }
    }
}