using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    public float speed;
    public float minDistance;
    Player player;
    Transform playerTransform;
    Transform myTransform;

    void Start()
    {
        myTransform = transform;
        player = FindObjectOfType<Player>();
        playerTransform = player.transform;
    }

    void LateUpdate()
    {
        if (player.CanDoStuff)
        {
            Vector2 distanceVector = playerTransform.position - myTransform.position;
            Vector2 direction = distanceVector.normalized;

            float x = direction.x * speed * Time.deltaTime;
            float y = direction.y * speed * Time.deltaTime;

            float xDistance = Mathf.Abs(distanceVector.x);

            Vector3 diff = Vector3.zero;

            if (xDistance > minDistance)
            {
                diff.x = x;
            }

            diff.y = y;

            myTransform.position += diff;
        }
    }
}
