using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private GameObject border1, border2, border3, border4;
    private readonly int BORDER_COUNT = 4;

    private void Start()
    {
        GameObject[] borders = GameObject.FindGameObjectsWithTag("ParticleBorder");
        if(borders != null)
        {
            if (borders.Length == BORDER_COUNT)
            {
                border1 = borders[0];
                border2 = borders[1];
                border3 = borders[2];
                border4 = borders[3];
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == border1 || collision.gameObject == border2 || collision.gameObject == border3 || collision.gameObject == border4)
        {
            Destroy(gameObject);
        }
    }
}
