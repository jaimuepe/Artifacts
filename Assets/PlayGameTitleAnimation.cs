using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameTitleAnimation : MonoBehaviour
{
    public float startTime;
    public float fadeTime;
    public float showTime;
    public SpriteRenderer srGameTitle;

    bool triggered = false;

    private void Start()
    {
        srGameTitle.color = new Color(1f, 1f, 1f, 0f);
        srGameTitle.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(FadeInGameTitle());
        }
    }

    IEnumerator FadeInGameTitle()
    {
        Player player = FindObjectOfType<Player>();
        player.DeactivateActions();

        yield return new WaitForSeconds(startTime);

        srGameTitle.gameObject.SetActive(true);
        float alpha = srGameTitle.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 1f, t));
            srGameTitle.color = newColor;
            yield return null;
        }
        srGameTitle.color = new Color(1, 1, 1, 1f);

        yield return new WaitForSeconds(showTime);
        StartCoroutine(FadeOutGameTitle());
    }

    IEnumerator FadeOutGameTitle()
    {
        float alpha = srGameTitle.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, t));
            srGameTitle.color = newColor;
            yield return null;
        }
        srGameTitle.color = new Color(1, 1, 1, 0f);
        srGameTitle.gameObject.SetActive(false);

        Player player = FindObjectOfType<Player>();
        player.ReactivateActions();
    }
}
