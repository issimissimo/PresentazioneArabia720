using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoPlayerCtrl : MonoBehaviour
{
    public Button playButton;
    public Button pauseButton;
    public Slider seekSlider;

    public static VideoPlayerCtrl instance;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {


        OnPlay();
    }

    public void Pause()
    {


        OnPause();
    }


    public void Seek()
    {
        // OnSeeking(time);
        float val = seekSlider.value;
        print(val);
    }


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
