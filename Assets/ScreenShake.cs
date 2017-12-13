using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Vector3 _originalPos;
    public static ScreenShake _instance;

    void Awake()
    {
        _originalPos = transform.localPosition;
        _instance = this;
    }

    public static void Shake(float duration, float amount)
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(_instance.ShakeCoroutine(duration, amount));
    }

    public static void Shake(float amount)
    {
        Shake(float.MaxValue, amount);
    }

    public static void Stop()
    {
        _instance.StopAllCoroutines();
    }

    IEnumerator ShakeCoroutine(float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            if (!GameManager.instance.gamePaused)
            {
                transform.localPosition = _originalPos + Random.insideUnitSphere * amount;
                duration -= Time.deltaTime;
            }

            yield return null;
        }

        transform.localPosition = _originalPos;
    }

}