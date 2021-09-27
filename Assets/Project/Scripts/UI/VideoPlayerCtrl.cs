using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VideoPlayerCtrl : MonoBehaviour
{
    public float openAnimDuration = 0.5f;
    // public Button _playButton;
    // public Button _pauseButton;
    public ToggleButton playPauseToggleButton;
    public ToggleButton loopToggleButton;
    public Slider seekSlider;

    public static event Action OnPLayEvent;
    public static event Action OnPauseEvent;
    public static event Action OnStartSeekEvent;
    public static event Action<float> OnSeekEvent;
    public static event Action OnEndSeekEvent;
    // public static event Action<bool> OnSetLoopEvent;

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

    private void Update()
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

    /// from UI buttons

    public void PlayPause()
    {
        if (!playPauseToggleButton.IsOn())
        {
            if (OnPauseEvent != null) OnPauseEvent.Invoke();
            print("PAUSE");
        }
        else
        {
            if (OnPLayEvent != null) OnPLayEvent.Invoke();
            print("PLAY");
        }
    }

    public void SetLooping()
    {
        GameManager.instance.ToggleVideoLoop(loopToggleButton.IsOn());
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
        print("ONPLAY");
        playPauseToggleButton.SetState(true);
    }

    public void OnPause()
    {
        print("ONPAUSE");
        playPauseToggleButton.SetState(false);
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
