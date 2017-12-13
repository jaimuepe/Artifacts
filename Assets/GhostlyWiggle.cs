using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostlyWiggle : MonoBehaviour
{

    public float xIntensity;
    public float yIntensity;

    public float frequency;

    Transform myTransform;

    float xPhase0;
    float yPhase0;

    private void Awake()
    {
        myTransform = transform;
        xPhase0 = Random.Range(0f, 2 * Mathf.PI);
        yPhase0 = Random.Range(0f, 2 * Mathf.PI);
    }

    void Update()
    {
        Vector3 position = myTransform.position;
        float x = xIntensity * Mathf.Cos(xPhase0 + frequency * Time.time) * Time.deltaTime;
        float y = yIntensity * Mathf.Sin(yPhase0 + frequency * Time.time) * Time.deltaTime;

        position.x += x;
        position.y += y;

        myTransform.position = position;
    }
}
