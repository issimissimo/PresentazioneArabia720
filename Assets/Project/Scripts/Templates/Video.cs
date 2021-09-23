using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public class Video : MonoBehaviour
{
    VideoPlayer videoPlayer;
    Coroutine PlayCoroutine;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play(Action callback)
    {
        if (PlayCoroutine != null)
        {
            StopCoroutine(PlayCoroutine);
        }
        PlayCoroutine = StartCoroutine(_Play(callback));
    }


    public void Stop()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
        if (PlayCoroutine != null)
        {
            StopCoroutine(PlayCoroutine);
        }
    }


    private IEnumerator _Play(Action callback)
    {
        /// UI
        VideoPlayerCtrl.instance.OnPlay();
        
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForEndOfFrame();
        }

        print(videoPlayer.length);

        videoPlayer.frame = 0;
        videoPlayer.Play();


        while (videoPlayer.isPlaying)
        {
            /// UI
            float seekValue = Mathf.InverseLerp(0, (float)videoPlayer.length, (float)videoPlayer.time);
            VideoPlayerCtrl.instance.OnSeeking(seekValue);

            yield return new WaitForEndOfFrame();
        }

        PlayCoroutine = null;

        if (callback != null) callback();

        /// UI
        VideoPlayerCtrl.instance.OnPause();
    }
}
