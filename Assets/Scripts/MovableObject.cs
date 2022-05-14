using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public bool InsideSomething;


    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("GridFloor"))
        {
            InsideSomething = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other)
        {
            InsideSomething = false;
        }
    }
}