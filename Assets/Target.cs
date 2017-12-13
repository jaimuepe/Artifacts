using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    Player player;
    Camera mainCamera;

    public float distance = 1f;

    SpriteRenderer sr;
    bool showing = false;

    public Vector2 currentDirection;

    Coroutine fadeInCoroutineRef;
    Coroutine fadeOutCoroutineRef;

    public float fadeOutStartTime;
    float fadeOutTimer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GetComponentInParent<Player>();
        mainCamera = Camera.main;
        showing = false;
    }

    void Update()
    {

        if (!player.CanDoStuff)
        {
            fadeOutTimer -= Time.deltaTime;
            if (fadeOutTimer <= 0)
            {
                Hide();
            }
            return;
        }

        if (GameManager.instance.usingKeyboardMouse)
        {
            Vector2 mousePosInWorldCoord = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2 direction = (mousePosInWorldCoord - playerPos).normalized;
            transform.localPosition = distance * new Vector3(direction.x, direction.y, 0f);

            currentDirection = direction;
            Show();
        }
        else
        {
            Vector2 controllerAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (Mathf.Abs(controllerAxis.x) != 0f ||
                Mathf.Abs(controllerAxis.y) != 0f)
            {
                Vector2 direction = controllerAxis.normalized;
                transform.localPosition = distance * new Vector3(direction.x, direction.y, 0f);

                currentDirection = direction;

                Show();
            }
            else
            {
                fadeOutTimer -= Time.deltaTime;
                if (fadeOutTimer <= 0)
                {
                    Hide();
                }
            }
        }
    }

    public void Hide()
    {
        if (fadeInCoroutineRef != null) { StopCoroutine(fadeInCoroutineRef); }

        if (showing)
        {

            fadeOutCoroutineRef = StartCoroutine(FadeOutCoroutine());
        }
    }

    public void Show()
    {
        fadeOutTimer = fadeOutStartTime;

        if (fadeOutCoroutineRef != null) { StopCoroutine(fadeOutCoroutineRef); }

        if (!showing)
        {

            fadeInCoroutineRef = StartCoroutine(FadeInCoroutine());
        }
    }

    IEnumerator FadeInCoroutine()
    {
        showing = true;

        float alpha = sr.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.3f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 1f, t));
            sr.color = newColor;
            yield return null;
        }
        sr.color = new Color(1, 1, 1, 1f);
    }

    IEnumerator FadeOutCoroutine()
    {
        showing = false;

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
