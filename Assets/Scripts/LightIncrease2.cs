using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Scripts
{
    public class LightIncrease2 : MonoBehaviour
    {
        [SerializeField] private Light2D _light2D;

        private void Start()
        {
            _light2D.intensity = .1f;
            StartCoroutine(IncreaseLight());
        }

        private IEnumerator IncreaseLight()
        {
            while (_light2D.intensity < .9f)
            {
                _light2D.intensity += Time.deltaTime;
                yield return null;
            }

            yield return null;
        }
    }
    
}