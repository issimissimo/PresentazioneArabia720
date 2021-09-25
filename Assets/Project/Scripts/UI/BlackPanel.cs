using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BlackPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Coroutine fadeCoroutine;
    public float animDuration = 0.5f;

    public static BlackPanel instance;

    private void Awake()
    {
        instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        // canvasGroup.alpha = 0;
    }

    public void Toggle(Action callback = null)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(Fade(0, 1, () =>
        {
            fadeCoroutine = StartCoroutine(Fade(1, 0, () =>
        {
            if (callback != null) callback();
        }));
        }));
    }

    public void FadeIn(Action callback = null)
    {
        print("FADEIN");
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(Fade(0, 1, () =>
        {
            if (callback != null) callback();
        }));
    }

    public void FadeOut(Action callback = null)
    {
        print("FADEOUT");
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(Fade(1, 0, () =>
        {
            if (callback != null) callback();
        }));
    }


    IEnumerator Fade(float initAlpha, float endAlpha, Action callback)
    {
        print("FADE");
        float time = Time.time;
        while (Time.time - time <= animDuration)
        {
            float t = (Time.time - time) / animDuration;
            t = t * t * (3f - 2f * t);
            canvasGroup.alpha = Mathf.Lerp(initAlpha, endAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        fadeCoroutine = null;
        print("ENDFADE");
        callback();
    }
}
