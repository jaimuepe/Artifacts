using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConstraintsDeactivator : MonoBehaviour
{
    public GameObject constraint;

    bool done;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!done && collider.CompareTag("Player"))
        {
            done = true;
            constraint.gameObject.SetActive(false);
        }
    }
}
