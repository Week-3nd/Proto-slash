using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public bool Colliding = false;

    void OnTriggerEnter(Collider other)
    {
        Colliding = true;
    }
}