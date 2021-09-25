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
    public static bool isFading;
    public static BlackPanel instance;
    private Action callback;

    private void Awake()
    {
        instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        // canvasGroup.alpha = 0;
    }

    // public void Toggle(Action _callback = null)
    // {
    //     if (fadeCoroutine != null)
    //     {
    //         StopCoroutine(fadeCoroutine);
    //         callback();
    //     }
    //     callback = _callback;
    //     fadeCoroutine = StartCoroutine(Fade(0, 1, () =>
    //     {
    //         fadeCoroutine = StartCoroutine(Fade(1, 0, () =>
    //     {
    //         if (callback != null) callback();
    //     }));
    //     }));
    // }

    public void FadeIn(Action _callback = null)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(Fade(0, 1, () =>
        {
            if (_callback != null) _callback();
        }));
    }

    public void FadeOut()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(Fade(1, 0));
    }


    IEnumerator Fade(float initAlpha, float endAlpha, Action _callback = null)
    {
        isFading = true;
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
        isFading = false;
        if (_callback != null) _callback();
    }
}
