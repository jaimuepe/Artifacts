using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyGhost : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player.CanDoStuff)
            {
                player.Kill(false);
            }
        }
    }
}
