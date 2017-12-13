using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    public float halfRespawnTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestoreToLastCheckpoint()
    {
        StartCoroutine(RestoreCheckpointCoroutine());
    }

    IEnumerator RestoreCheckpointCoroutine()
    {
        GameManager.instance.FadeOutGame();
        yield return new WaitForSeconds(GameManager.instance.fadeTime);
        yield return new WaitForSeconds(halfRespawnTime);

        Player player = FindObjectOfType<Player>();
        player.RestoreCheckpoint();

        Transform playerTransform = player.transform;
        float playerX = playerTransform.position.x;
        float playerY = playerTransform.position.y;

        float playerCellX = 19.2f * (int) (playerX / 19.2f);
        float playerCellY = 10.8f * (int)(playerY / 10.8f);

        FollowPlayer cameraWrapper = FindObjectOfType<FollowPlayer>();
        cameraWrapper.transform.position = new Vector3(playerCellX, playerCellY, cameraWrapper.transform.position.z);

        if (GameManager.instance.bossFightReady)
        {
            Boss boss = GameManager.instance.boss;
            boss.Restore();

            yield return new WaitForSeconds(halfRespawnTime);
            GameManager.instance.FadeInGame();
            yield break;
        }

        int checkpointId = GameManager.instance.currentCheckpoint;

        List<LesserArtifact> collectedLesserArtifacts =new List<LesserArtifact>(
            GameManager.instance.collectedLesserArtifacts);

        for (int i = 0; i < collectedLesserArtifacts.Count; i++)
        {
            if (!GameManager.instance.savedLesserArtifacts.Contains(collectedLesserArtifacts[i]))
            {
                collectedLesserArtifacts[i].Restore();
            }
        }

        GameManager.instance.ClearCollectedLesserArtifacts();

        if (!GameManager.instance.lastArtifactCollected)
        {
            Ghost[] ghosts = GameManager.instance.ghosts;
            for (int i = 0; i < ghosts.Length; i++)
            {
                if (ghosts[i].artifactId <= checkpointId)
                {
                    ghosts[i].Restore();
                }
            }
        }

        yield return new WaitForSeconds(halfRespawnTime);
        GameManager.instance.FadeInGame();
    }
}
