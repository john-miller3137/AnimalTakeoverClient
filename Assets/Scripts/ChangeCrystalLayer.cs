using System;
using UnityEngine;

namespace Scripts
{
    public class ChangeCrystalLayer : MonoBehaviour
    {
        private void OnCollisionStay(Collision other)
        {
            Debug.Log("hit");
            if (other.gameObject.layer == 10)
            {
                Debug.Log("hit");
            }
        }

        
    }
}