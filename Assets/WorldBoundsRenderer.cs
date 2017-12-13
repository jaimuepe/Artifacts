using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBoundsRenderer : MonoBehaviour
{

    public float size;

    public Vector3 up;
    public Vector3 left;
    public Vector3 down;
    public Vector3 right;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(left + size * Vector3.down, left + size * Vector3.up);
        Gizmos.DrawLine(right + size * Vector3.down, right + size * Vector3.up);
        Gizmos.DrawLine(down + size * Vector3.right, down + size * Vector3.left);
        Gizmos.DrawLine(up + size * Vector3.right, up + size * Vector3.left);
    }
}
