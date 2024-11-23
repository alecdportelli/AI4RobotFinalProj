using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robots
{
    public class CollisionDetector : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject);
            // Check if the colliding object is the ground
            if (collision.gameObject.CompareTag("Ground"))
            {
                Debug.Log($"{gameObject.name} collided with the ground!");
            }
        }
    }
}
