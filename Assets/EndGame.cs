using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    bool started = false;

    public Image endPanel;
    public FMODUnity.StudioEventEmitter musicEmitter;
    public FMODUnity.StudioEventEmitter ambientNoiseEmitter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!started && GameManager.instance.bossFightReady && collision.CompareTag("Player"))
        {
            started = true;
            Boss boss = FindObjectOfType<Boss>();
            boss.DisableAndFadeOut();
            StartCoroutine(EndGameCoroutine());
        }
    }

    IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(1f);

        float alpha = endPanel.color.a;
        for (float t = 0.0f; t < 3.0f; t += Time.deltaTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 1f, t));
            endPanel.color = newColor;
            yield return null;
        }
        endPanel.color = new Color(1, 1, 1, 1f);

        musicEmitter.Stop();
        ambientNoiseEmitter.Stop();

        if (GameManager.instance.collectedLesserArtifacts.Count == GameManager.instance.lesserArtifacts.Length)
        {
            SceneManager.LoadScene("endgame_b");
        }
        else
        {
            SceneManager.LoadScene("endgame_a");
        }
    }
}
