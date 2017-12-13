using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionCheck : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RegionMask"))
        {
            FindObjectOfType<GameMasking>().SwapMask(collision.GetComponent<GameMask>());
        }
    }
}
