using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastArtifact : MonoBehaviour
{

    public Vector3 defaultPosition;
    public float elevatorDelay;

    public void Collect()
    {
        Ghost[] ghosts = GameManager.instance.ghosts;
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].HideBeforeBossFight();
        }

        GameManager.instance.CollectLastArtifact();

        StartCoroutine(ElevatorUp());

        Player player = FindObjectOfType<Player>();
        player.DeactivateActions();
        transform.position = new Vector3(-100000, -100000, transform.position.z);
    }

    IEnumerator ElevatorUp()
    {
        yield return new WaitForSeconds(elevatorDelay);
        Elevator elevator = FindObjectOfType<Elevator>();
        elevator.GoUp();
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
        UnityEditor.Handles.Label(transform.position + 2 * Vector3.right, "Last artifact");
#endif
    }
}
