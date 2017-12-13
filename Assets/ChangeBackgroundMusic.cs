using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackgroundMusic : MonoBehaviour
{

    public FMODUnity.StudioEventEmitter bgMusicEmitter;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bgMusicEmitter.SetParameter("alienZone", 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bgMusicEmitter.SetParameter("alienZone", 2f);
        }
    }
}
