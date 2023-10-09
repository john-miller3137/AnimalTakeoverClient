using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveToPosition(gameObject, new Vector3(1, 5, 0), 5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MoveToPosition(GameObject animal, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;
        Vector3 startingPos = animal.transform.position;

        while (elapsedTime < duration)
        {
            animal.transform.position = Vector3.Lerp(startingPos, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = targetPosition;

        yield return null;
    }
}
