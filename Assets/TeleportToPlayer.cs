using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToPlayer : MonoBehaviour
{

    Transform myTransform;
    Transform playerTransform;

    public float distanceToTeleport;

    public float minDistanceAfterTeleport;
    public float maxDistanceAfterTeleport;

    void Start()
    {
        myTransform = transform;
        playerTransform = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        if (Vector2.Distance(myTransform.position, playerTransform.position) > distanceToTeleport)
        {
            Vector3 playerPos = playerTransform.position;
            float angle = Random.Range(0, 2 * Mathf.PI);
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            float distance = Random.Range(minDistanceAfterTeleport, maxDistanceAfterTeleport);
            myTransform.position = playerPos + new Vector3(distance * cos, distance * sin, myTransform.position.z);
        }
    }
}
