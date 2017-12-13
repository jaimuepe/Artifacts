using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeTargetRenderer : MonoBehaviour
{
    Target target;

    Transform playerTransform;
    Transform targetTransform;

    public LayerMask terrainLayer;

    LineRenderer lr;

    void Start()
    {
        target = GetComponentInParent<Target>();
        playerTransform = FindObjectOfType<Player>().transform;
        targetTransform = target.transform;

        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {

        Vector3 source = playerTransform.position;
        Vector3 direction = (targetTransform.position - source).normalized;

        Vector3 collisionPoint = source + 20f * direction;
        RaycastHit2D hit = Physics2D.Raycast(targetTransform.position, direction, 20f, terrainLayer);
        if (hit)
        {
            collisionPoint = hit.point;
        }

        lr.material.SetTextureOffset("_MainTex", -new Vector2(Time.timeSinceLevelLoad * 2f, 0f));
        lr.SetPositions(new Vector3[] { source, collisionPoint });
    }
}
