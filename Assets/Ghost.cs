using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public int artifactId;
    public Vector3 defaultPosition;

    public float fadeTime;

    SpriteRenderer sr;

    FMODUnity.StudioEventEmitter emitter;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void Start()
    {
        defaultPosition = transform.position;
    }

    public void Restore()
    {
        ChargeAttack chargeAttack = GetComponent<ChargeAttack>();
        if (chargeAttack)
        {
            chargeAttack.Restore();
        }

        sr.color = new Color(1, 1, 1, 0f);
        gameObject.SetActive(true);
        transform.position = defaultPosition;

        StopAllCoroutines();
        StartCoroutine(FadeIn());
        emitter.Play();
    }

    public void HideBeforeBossFight()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(FadeOutBeforeBossFight());
        }
    }

    IEnumerator FadeIn()
    {
        float alpha = sr.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 1f, t));
            sr.color = newColor;
            yield return null;
        }
        sr.color = new Color(1, 1, 1, 1f);
    }

    IEnumerator FadeOutBeforeBossFight()
    {
        float alpha = sr.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, t));
            sr.color = newColor;
            yield return null;
        }
        sr.color = new Color(1, 1, 1, 0f);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position, "Ghost @ artifact " + artifactId);
#endif
    }
}
