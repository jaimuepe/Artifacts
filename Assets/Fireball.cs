using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    public Vector2 direction;
    public float maxDistance;

    Transform myTransform;

    float distance = 0f;

    void Start()
    {
        myTransform = transform;
    }

    void Update()
    {
        Vector2 dv = direction * speed * Time.deltaTime;

        distance += dv.magnitude;

        if (distance > maxDistance)
        {
            Destroy(gameObject);
        }
        else
        {
            myTransform.position += new Vector3(dv.x, dv.y, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player.CanDoStuff)
            {
                collision.GetComponent<Player>().Kill(false);
            }
        }
        Destroy(gameObject);
    }
}
