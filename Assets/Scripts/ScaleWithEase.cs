using System.Collections;
using UnityEngine;

namespace Scripts
{
    using UnityEngine;

    public class ScaleWithEase : MonoBehaviour
    {
        public float duration = 1f;  // Total duration of each scaling animation

        private Vector3 initialScale;
        private SpriteRenderer sr;
        private Vector3 targetScale;
        private float elapsedTime;
        private bool scalingUp = true;  // Flag to track the current scaling direction
        public bool doScaling;
        
        private void Start()
        {
            ;
        }
        

        
    }
}