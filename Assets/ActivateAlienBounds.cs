using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAlienBounds : MonoBehaviour
{
    public Collider2D alienBounds;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            alienBounds.gameObject.SetActive(true);
        }
    }
}
