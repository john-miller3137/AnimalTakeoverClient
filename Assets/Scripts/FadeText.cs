using System;
using System.Collections;
using TMPro;
using UnityEngine;
using TextAsset = UnityEngine.TextCore.Text.TextAsset;

namespace Scripts
{
    public class FadeText : MonoBehaviour
    {
        public float fadeIn = .1f;
        public float fadeOut = .2f;
        public float solidColorTime = .4f;
        private void Start()
        {
            StartCoroutine(FadeTextToFullAlpha(fadeIn, gameObject.GetComponent<TextMeshProUGUI>()));
            StartCoroutine(DoFadeText(fadeOut));
        }

        public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
        {
            while (i.color.a < 1.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
                i.outlineColor = new Color(i.outlineColor.r, i.outlineColor.g, i.outlineColor.b, i.outlineColor.a + (Time.deltaTime / t));
                yield return null;
            }
        }
        private IEnumerator DoFadeText(float t)
        {
            yield return new WaitForSeconds(solidColorTime);
            TextMeshProUGUI i = gameObject.GetComponent<TextMeshProUGUI>();
            while (i.color.a > 0.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
                i.outlineColor = new Color(i.outlineColor.r, i.outlineColor.g, i.outlineColor.b, i.outlineColor.a - (Time.deltaTime / t));
                yield return null;
            }
            yield return null;
        }
    }
}