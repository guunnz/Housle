using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBehaviour : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("GridWallLeft") || !other.CompareTag("GridWallRight"))
            Destroy(other.gameObject);
    }
}
