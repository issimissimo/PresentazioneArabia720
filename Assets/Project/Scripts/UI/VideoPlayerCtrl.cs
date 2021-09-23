using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VideoPlayerCtrl : MonoBehaviour
{
    public Button playButton;
    public Button pauseButton;
    public Slider seekSlider;

    public event Action OnPLayEvent;
    public event Action OnPauseEvent;
    public event Action OnStartSeekEvent;
    public event Action<float> OnSeekEvent;
    public event Action OnEndSeekEvent;

    public static VideoPlayerCtrl instance;

    private void Awake()
    {
        instance = this;
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
}
