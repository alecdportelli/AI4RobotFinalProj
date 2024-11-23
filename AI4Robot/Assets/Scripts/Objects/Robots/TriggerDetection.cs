using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robots
{
    public class TriggerDetection : MonoBehaviour
    {
        private bool isTriggered = false;

        // Called when the collider enters a trigger zone
        private void OnTriggerStay(Collider other)
        {
            isTriggered = true;
        }

        // Called when the collider exits a trigger zone
        private void OnTriggerExit(Collider other)
        {
            isTriggered = false;
        }

        // Public method to get the current trigger state
        public bool IsTriggered()
        {
            return isTriggered;
        }
    }
}
