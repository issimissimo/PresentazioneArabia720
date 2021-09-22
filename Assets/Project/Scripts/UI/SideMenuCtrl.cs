using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenuCtrl : MonoBehaviour
{
    RectTransform rect;
    float xInit;
    float yInit;
    float xSize;
    public float openAnimDuration = 1f;

    Coroutine _MovePanel;
    bool isOpen;

    public bool hideOnStart = true;



    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        xInit = rect.anchoredPosition.x;
        yInit = rect.anchoredPosition.y;
        xSize = rect.sizeDelta.x;
    }


    void Start()
    {
        if (hideOnStart)
            HideNow();
    }



    public void HideNow()
    {
        rect.anchoredPosition = new Vector2(xSize, yInit);
    }



    public void Toggle()
    {
        Vector2 initPos = new Vector2(rect.anchoredPosition.x, yInit);
        Vector2 endPos;

        if (_MovePanel != null)
        {
            StopCoroutine(_MovePanel);
        }

        if (!isOpen)
        {
            isOpen = true;
            endPos = new Vector2(xInit, yInit);
        }
        else
        {
            isOpen = false;
            endPos = new Vector2(xSize, yInit);
        }

        _MovePanel = StartCoroutine(_Move(initPos, endPos));
    }



    IEnumerator _Move(Vector2 initPos, Vector2 endPos)
    {
        float time = Time.time;
        while (Time.time - time <= openAnimDuration)
        {
            float t = (Time.time - time) / openAnimDuration;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            rect.anchoredPosition = Vector2.Lerp(initPos, endPos, t);
            yield return null;
        }

        rect.anchoredPosition = endPos;
        _MovePanel = null;
    }
}
