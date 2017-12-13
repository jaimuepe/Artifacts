using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    SpriteRenderer sr;

    public FMODUnity.StudioEventEmitter emitterStart;
    public FMODUnity.StudioEventEmitter emitterLoop;
    public FMODUnity.StudioEventEmitter bgMusic;

    public ChasePlayer chasePlayerComponent;

    Vector3 defaultPosition;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
        gameObject.SetActive(false);

        defaultPosition = transform.position;
    }

    public void Restore()
    {
        if (!gameObject.activeSelf) { return; }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
        transform.position = defaultPosition;
        emitterStart.Stop();
        emitterLoop.Stop();
        gameObject.SetActive(false);

        Player player = FindObjectOfType<Player>();
        player.DeactivateActions();
    }

    public void DisableAndFadeOut()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        StartCoroutine(FadeOut());
        emitterStart.Stop();
        emitterLoop.Stop();
        bgMusic.Stop();
    }

    public void ShowUp()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        emitterStart.Play();
        float alpha = sr.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 1f, t));
            sr.color = newColor;
            yield return null;
        }
        sr.color = new Color(1, 1, 1, 1f);
        emitterLoop.Play();

        yield return new WaitForSeconds(1f);
        chasePlayerComponent.enabled = true;

        Player player = FindObjectOfType<Player>();
        player.ReactivateActions();
    }

    IEnumerator FadeOut()
    {
        float alpha = sr.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, t));
            sr.color = newColor;
            yield return null;
        }
        sr.color = new Color(1, 1, 1, 0f);
    }
}
