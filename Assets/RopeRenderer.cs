using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{

    LineRenderer lr;
    public Vector3 anchorPoint;

    Rope rope;
    Transform myTransform;

    void Start()
    {
        myTransform = transform;
        rope = GetComponentInParent<Rope>();

        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
    }

    private void Update()
    {
        lr.SetPosition(0, new Vector3(rope.transform.position.x, rope.transform.position.y, myTransform.position.z));
        lr.SetPosition(1, new Vector3(anchorPoint.x, anchorPoint.y, myTransform.position.z));
    }

}
