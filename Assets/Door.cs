using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float shakeStrength;

    public Vector3 targetPosition;

    FMODUnity.StudioEventEmitter doorSoundEmitter;

    Transform myTransform;

    private void Awake()
    {
        doorSoundEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void Start()
    {
        myTransform = transform;
    }

    public void Open(float animationTime)
    {
        FollowPlayer followPlayer = FindObjectOfType<FollowPlayer>();
        followPlayer.FocusOnSomethingElse(transform, 1f);
        ScreenShake.Shake(shakeStrength);
        doorSoundEmitter.Play();
        StartCoroutine(OpenDoorCoroutine(animationTime));
    }

    IEnumerator OpenDoorCoroutine(float animationTime)
    {
        Vector3 position = myTransform.localPosition;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / animationTime)
        {
            Vector2 diff = Vector2.Lerp(position, targetPosition, t);
            myTransform.localPosition = new Vector3(diff.x, diff.y, myTransform.localPosition.z);
            yield return null;
        }

        ScreenShake.Stop();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + targetPosition, Vector3.one);
    }
}
