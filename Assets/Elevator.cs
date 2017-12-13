using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    Transform myTransform;
    public float increment;
    public float screenShake;

    FMODUnity.StudioEventEmitter emitter;

    private void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        myTransform = transform;
    }

    public void GoUp()
    {
        ScreenShake.Shake(screenShake);
        emitter.Play();
        StartCoroutine(GoUpCoroutine());
    }

    IEnumerator GoUpCoroutine()
    {
        while (true)
        {
            myTransform.position += new Vector3(0f, increment * Time.deltaTime, 0f);
            yield return null;
        }
    }

    public GameObject zone2;
    public GameObject zone2b;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ElevatorStop"))
        {
            ScreenShake.Stop();
            emitter.SetParameter("towerFinish", 1f);
            StopAllCoroutines();
            Player player = FindObjectOfType<Player>();
            player.ReactivateActions();
            GameManager.instance.bossFightReady = true;

            zone2.gameObject.SetActive(false);
            zone2b.gameObject.SetActive(true);

            zone2b.transform.position = zone2.transform.position;
        }
    }
}

