using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAndPlayMusic : MonoBehaviour
{
    FMODUnity.StudioEventEmitter emitter;

    public Image panel;

    private void Start()
    {
        StartCoroutine(FadeOut());
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        emitter.Play();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Application.Quit();
        }
    }

    IEnumerator FadeOut()
    {
        float alpha = panel.color.a;
        for (float t = 0.0f; t < 3.0f; t += Time.deltaTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, t));
            panel.color = newColor;
            yield return null;
        }
        panel.color = new Color(1, 1, 1, 0f);
    }
}
