using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFireballs : MonoBehaviour
{
    public Fireball fireballPref;

    Transform myTransform;
    Transform playerTransform;

    public float minFireDistance;
    public float fireRateInSeconds;

    float fireRateCounter;

    public FMODUnity.StudioEventEmitter emitter;

    private void Start()
    {
        myTransform = transform;
        playerTransform = FindObjectOfType<Player>().transform;
        fireRateCounter = fireRateInSeconds;
    }

    private void Update()
    {
        float distance = Vector2.Distance(myTransform.position, playerTransform.position);
        if (distance < minFireDistance && fireRateCounter <= 0)
        {
            Shoot();
        }

        fireRateCounter -= Time.deltaTime;
    }

    void Shoot()
    {
        fireRateCounter = fireRateInSeconds;
        Fireball fireBall = Instantiate(fireballPref);
        fireBall.transform.position = myTransform.position;
        Vector2 direction = new Vector2(playerTransform.position.x - myTransform.position.x,
            playerTransform.position.y - myTransform.position.y).normalized;

        fireBall.direction = direction;

        emitter.Play();
    }
}
