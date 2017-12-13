using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWall : MonoBehaviour
{

    bool discovered = false;
    SpriteRenderer sr;

    Collider2D polygonCollider;

    Collider2D playerCollider;

    private void Awake()
    {
        polygonCollider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }

    private void Start()
    {
        playerCollider = FindObjectOfType<Player>().GetComponent<BoxCollider2D>();
    }

    bool visible = true;

    private void Update()
    {
        if (polygonCollider.IsTouching(playerCollider))
        {
            if (visible)
            {
                Hide();
            }
        }
        else
        {
            if (!visible)
            {
                Show();
            }
        }
    }

    void Show()
    {
        StopAllCoroutines();
        StartCoroutine(ShowCoroutine());
    }

    void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(HideCoroutine());
    }

    IEnumerator ShowCoroutine()
    {
        visible = true;

        float alpha = sr.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.3f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 1f, t));
            sr.color = newColor;
            yield return null;
        }
        sr.color = new Color(1, 1, 1, 1f);
    }

    IEnumerator HideCoroutine()
    {
        visible = false;

        float alpha = sr.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.3f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, t));
            sr.color = newColor;
            yield return null;
        }
        sr.color = new Color(1, 1, 1, 0f);
    }
}
