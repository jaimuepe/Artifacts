using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardsPlayer : MonoBehaviour
{

    Transform player;
    Transform myTransform;

    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        myTransform = transform;
        player = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        if (player.position.x >= myTransform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}
