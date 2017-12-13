using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGround : MonoBehaviour
{

    float duration;

    public float shakeStrength;
    public GameObject destroyObject;

    public FMODUnity.StudioEventEmitter emitter;

    private void Start()
    {
        Artifact artifact = GetComponent<Artifact>();
        duration = artifact.disableActionsDelay;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(BreakGroundCoroutine());
        }
    }

    IEnumerator BreakGroundCoroutine()
    {
        ScreenShake.Shake(shakeStrength);
        emitter.Play();
        yield return new WaitForSeconds(duration);
        Destroy(destroyObject);
        ScreenShake.Stop();
    }
}
