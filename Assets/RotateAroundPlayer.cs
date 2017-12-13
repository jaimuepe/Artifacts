using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundPlayer : MonoBehaviour
{
    Player player;

    Transform myTransform;
    Transform playerTransform;

    public float minDistance;
    public float frequency;

    public float speed;

    private void Awake()
    {
        myTransform = transform;
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        playerTransform = player.transform;
    }

    void Update()
    {
        Vector3 position = myTransform.position;

        Vector2 distanceVector = playerTransform.position - myTransform.position;
        Vector2 direction = distanceVector.normalized;

        float xDistance = Mathf.Abs(distanceVector.x);
        float yDistance = Mathf.Abs(distanceVector.y);

        Vector3 diff = Vector3.zero;

        if (xDistance > minDistance)
        {
            float x = direction.x * speed * Time.deltaTime;
            diff.x = x;
        }

        if (yDistance > minDistance)
        {
            float y = direction.y * speed * Time.deltaTime;
            diff.y = y;
        }

        position += diff;

        float phi = frequency * Time.deltaTime;
        float cos = Mathf.Cos(phi);
        float sin = Mathf.Sin(phi);

        position.x -= playerTransform.position.x;
        position.y -= playerTransform.position.y;

        float xx = position.x * cos - position.y * sin;
        float yy = position.x * sin + position.y * cos;

        position.x = xx + playerTransform.position.x;
        position.y = yy + playerTransform.position.y;

        myTransform.position = position;
    }
}
