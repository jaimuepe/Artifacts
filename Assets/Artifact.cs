using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    public int artifactId;
    public Door door;
    public Ghost ghost;

    public bool collected = false;

    [Header("Animation sequence")]
    public float doorAnimationDelay;
    public float disableActionsDelay;
    public float focusOnGhostDelay;
    public float spawnGhostDelay;

    public GameObject destroyCameraConstraint;

    public Vector3 defaultPosition;

    public bool useStartPosition;

    private void Start()
    {
        if (useStartPosition)
        {
            defaultPosition = transform.position;
        }
    }

    void Collect()
    {
        collected = true;
        GameManager.instance.Add(this);

        if (destroyCameraConstraint != null)
        {
            destroyCameraConstraint.gameObject.SetActive(false);
        }

        if (door)
        {
            door.Open(doorAnimationDelay);
        }
        StartCoroutine(SpawnGhost());
        StartCoroutine(DisableActions());

        transform.position = new Vector3(-100000, -100000, transform.position.z);
    }

    IEnumerator SpawnGhost()
    {
        yield return new WaitForSeconds(focusOnGhostDelay);
        FollowPlayer.instance.FocusOnSomethingElse(ghost.transform, 1f);
        yield return new WaitForSeconds(spawnGhostDelay);
        ghost.Restore();
    }

    IEnumerator DisableActions()
    {
        Player player = FindObjectOfType<Player>();
        player.DeactivateActions();
        yield return new WaitForSeconds(disableActionsDelay);
        player.ReactivateActions();
        FollowPlayer.instance.FocusOnPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + 2 * Vector3.right, "Artifact " + artifactId);
#endif
    }
}
