using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBehaviour : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Window"))
            Destroy(other.gameObject);
    }
}
