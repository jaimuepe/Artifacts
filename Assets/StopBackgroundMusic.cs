using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBackgroundMusic : MonoBehaviour {

    public FMODUnity.StudioEventEmitter bgMusicEmitter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bgMusicEmitter.Stop();
        }
    }
}
