using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    public bool alive = true;
    public float restoreDelay;

    public void KillPlayer()
    {
        if (!alive) { return; }

        alive = false;
        Player player = FindObjectOfType<Player>();
        player.rope.BreakRope();
        StartCoroutine(RestoreLastCheckpoint());
    }

    IEnumerator RestoreLastCheckpoint()
    {
        yield return new WaitForSeconds(restoreDelay);
        CheckpointManager.instance.RestoreToLastCheckpoint();
    }
}
