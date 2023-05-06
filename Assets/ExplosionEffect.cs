using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ExplosionEffect : MonoBehaviour
{
    public float explosionForce = 5.0f;
    public float explosionDuration = 1.0f;
    public GameObject fireworkParticlePrefab;
    List<GameObject> particles;
    Light2D worldLight;
    [SerializeField] private GameObject WorldLight;

    public void Start()
    {
        if(WorldLight != null)
        {
            worldLight = WorldLight.GetComponent<Light2D>();
        }
    }

    public IEnumerator ExplodeCoroutine()
    {
        Vector3 explosionPosition = transform.position;
        Destroy(gameObject);
        particles = new List<GameObject>();
        for (int i = 0; i < 12; i++)
        {
            Vector3 direction = Random.insideUnitSphere;
            GameObject particle = Instantiate(fireworkParticlePrefab, explosionPosition, Quaternion.identity);
            Rigidbody2D rb2D = particle.GetComponent<Rigidbody2D>();
            rb2D.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            Light2D light2D = particle.GetComponent<Light2D>();
            if(i < 6)
            {
                light2D.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b, 1);
            } else
            {
                light2D.color = new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, Color.magenta.a);
                
            }
            
            particles.Add(particle);

        }

        worldLight.intensity = 1.1f;
        yield return null;
        
    }
    
}