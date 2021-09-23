using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TopBarCtrl : MonoBehaviour
{
    bool mouseIsOver;
    CanvasGroup canvasGroup;
    Coroutine ToggleCoroutine;
    string gameobjectName;
    public bool hideOnStart = true;
    public float openAnimDuration = 0.5f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        gameobjectName = gameObject.name;
    }

    void Start()
    {
        if (hideOnStart)
            canvasGroup.alpha = 0;
    }

    void Update()
    {
        if (!mouseIsOver && Utils.IsPointerOverUI(gameobjectName))
        {
            mouseIsOver = true;
            Toggle();
        }
        else if (mouseIsOver && !Utils.IsPointerOverUI(gameobjectName))
        {
            mouseIsOver = false;
            Toggle();
        }
    }


    public void Toggle()
    {
        float initAlpha = canvasGroup.alpha;
        float endAlpha = mouseIsOver ? 1 : 0;
        if (ToggleCoroutine != null)
        {
            StopCoroutine(ToggleCoroutine);
        }
        ToggleCoroutine = StartCoroutine(_Toggle(initAlpha, endAlpha));
    }


    IEnumerator _Toggle(float initAlpha, float endAlpha)
    {
        float time = Time.time;
        while (Time.time - time <= openAnimDuration)
        {
            float t = (Time.time - time) / openAnimDuration;
            t = t * t * (3f - 2f * t);
            canvasGroup.alpha = Mathf.Lerp(initAlpha, endAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        ToggleCoroutine = null;
    }
}
