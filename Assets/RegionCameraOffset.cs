using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionCameraOffset : MonoBehaviour
{
    public float smoothTime = 0.1f;

    public Vector2 onEnterOffset;
    public Vector2 onExitOffset;

    public Transform interestPoint;
    FollowPlayer cameraWrapper;

    public bool followPlayerOnExit = true;

    bool active = false;

    Player player;

    public bool onEnterFollowHorizontally;
    public bool onEnterFollowVertically;

    public bool onExitFollowHorizontally;
    public bool onExitFollowVertically;

    private void Start()
    {
        cameraWrapper = FindObjectOfType<FollowPlayer>();
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!active)
            {
                active = true;
                if (interestPoint.gameObject.GetInstanceID() == player.gameObject.GetInstanceID())
                {
                    cameraWrapper.FocusOnPlayer(onEnterOffset, onEnterFollowHorizontally, onEnterFollowVertically);
                }
                else
                {
                    cameraWrapper.FocusOnSomethingElse(interestPoint, smoothTime);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            active = false;
            if (followPlayerOnExit)
            {
                cameraWrapper.FocusOnPlayer(onExitOffset, onExitFollowHorizontally, onExitFollowVertically);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(interestPoint.position, 1f);
#if UNITY_EDITOR
        UnityEditor.Handles.Label(interestPoint.position, "INTEREST POINT");
#endif
    }
}
