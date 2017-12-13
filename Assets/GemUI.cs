using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemUI : MonoBehaviour
{
    Image image;
    public float animationFrameTime;
    public Sprite[] sprites;
    int counter = 0;

    void Start()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (image == null) { image = GetComponent<Image>(); }
        StartCoroutine(UpdateIgnoreTimeScale());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        counter = 0;
    }

    IEnumerator UpdateIgnoreTimeScale()
    {
        while (true)
        {
            counter = (counter + 1) % sprites.Length;
            image.sprite = sprites[counter];
            yield return new WaitForSecondsRealtime(animationFrameTime);
        }
    }
}
