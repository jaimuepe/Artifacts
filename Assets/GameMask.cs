using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMask : MonoBehaviour
{

    SpriteRenderer sr;

    public float fadeTime;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        sr.color = new Color(c.r, c.g, c.b, 1f);
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine());
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        float alpha = sr.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, 0f, t));
            sr.color = newColor;
            yield return null;
        }
        sr.color = new Color(0, 0, 0, 0f);
    }

    IEnumerator FadeOutCoroutine()
    {
        float alpha = sr.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, 1f, t));
            sr.color = newColor;
            yield return null;
        }
        sr.color = new Color(0, 0, 0, 1f);
    }
}
