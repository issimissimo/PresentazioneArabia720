using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VideoPlayerCtrl : MonoBehaviour
{
    public float openAnimDuration = 0.5f;
    public Button playButton;
    public Button pauseButton;
    public Slider seekSlider;

    public event Action OnPLayEvent;
    public event Action OnPauseEvent;
    public event Action OnStartSeekEvent;
    public event Action<float> OnSeekEvent;
    public event Action OnEndSeekEvent;

    public static VideoPlayerCtrl instance;

    CanvasGroup canvasGroup;
    Coroutine ToggleCoroutine;
    bool mouseIsOver;
    string gameobjectName;

    private void Awake()
    {
        instance = this;
        gameobjectName = gameObject.name;
        canvasGroup = GetComponent<CanvasGroup>();
    }

     void Start()
    {
        canvasGroup.alpha = 0;
    }

    private void Update() {
        if (!mouseIsOver && Utils.IsPointerOverUI(gameobjectName))
        {
            mouseIsOver = true;
            Toggle();
        }
        else if(mouseIsOver && !Utils.IsPointerOverUI(gameobjectName)){
            mouseIsOver = false;
            Toggle();
        }
    }

    /// from UI buttons

    public void Play()
    {
        if (OnPLayEvent != null) OnPLayEvent.Invoke();
        OnPlay();
    }

    public void Pause()
    {
        if (OnPauseEvent != null) OnPauseEvent.Invoke();
        OnPause();
    }

    public void StartSeek()
    {
        if (OnStartSeekEvent != null) OnStartSeekEvent.Invoke();
    }

    public void Seek()
    {
        float val = seekSlider.value;
        if (OnSeekEvent != null) OnSeekEvent.Invoke(val);
    }

    public void EndSeek()
    {
        if (OnEndSeekEvent != null) OnEndSeekEvent.Invoke();
    }


    /// 

    public void OnPlay()
    {
        playButton.interactable = false;
        pauseButton.interactable = true;
    }

    public void OnPause()
    {
        playButton.interactable = true;
        pauseButton.interactable = false;
    }

    public void OnSeeking(float value)
    {
        seekSlider.value = value;
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
