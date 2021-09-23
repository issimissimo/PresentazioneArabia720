using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public class Video : MonoBehaviour
{
    VideoPlayer videoPlayer;
    Coroutine PlayCoroutine;
    Action onVideoFinishedCallback;
    bool isPlaying;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void OnEnable()
    {
        VideoPlayerCtrl.instance.OnPLayEvent += OnPlayFromUI;
        VideoPlayerCtrl.instance.OnPauseEvent += OnPauseFromUI;
        VideoPlayerCtrl.instance.OnStartSeekEvent += OnStartSeekFromUI;
        VideoPlayerCtrl.instance.OnSeekEvent += OnSeekFromUI;
        VideoPlayerCtrl.instance.OnEndSeekEvent += OnEndSeekFromUI;
        videoPlayer.loopPointReached += OnVideoFinished;
    }


    private void OnDisable()
    {
        VideoPlayerCtrl.instance.OnPLayEvent -= OnPlayFromUI;
        VideoPlayerCtrl.instance.OnPauseEvent -= OnPauseFromUI;
        VideoPlayerCtrl.instance.OnStartSeekEvent -= OnStartSeekFromUI;
        VideoPlayerCtrl.instance.OnSeekEvent -= OnSeekFromUI;
        VideoPlayerCtrl.instance.OnEndSeekEvent -= OnEndSeekFromUI;
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void Update()
    {
        /// UI
        if (videoPlayer.isPrepared && videoPlayer.isPlaying)
        {
            float seekValue = Mathf.InverseLerp(0, (float)videoPlayer.length, (float)videoPlayer.time);
            VideoPlayerCtrl.instance.OnSeeking(seekValue);
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        print("END");
        isPlaying = false;
        if (onVideoFinishedCallback != null) onVideoFinishedCallback();
    }

    private void OnPlayFromUI()
    {
        videoPlayer.Play();
    }

    private void OnPauseFromUI()
    {
        videoPlayer.Pause();
    }

    private void OnStartSeekFromUI()
    {
        isPlaying = videoPlayer.isPlaying ? true : false;
        if (isPlaying) videoPlayer.Pause();
    }

    private void OnSeekFromUI(float val)
    {
        float frame = Mathf.Lerp(0, (float)videoPlayer.frameCount, val);
        videoPlayer.frame = (int)frame;
    }

    private void OnEndSeekFromUI()
    {
        if (isPlaying) videoPlayer.Play();
    }


    public void Play(Action callback)
    {
        onVideoFinishedCallback = callback;

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

        // print(videoPlayer.length);

        videoPlayer.frame = 0;
        videoPlayer.Play();


        // while (videoPlayer.isPlaying)
        // {
        //     /// UI
        //     float seekValue = Mathf.InverseLerp(0, (float)videoPlayer.length, (float)videoPlayer.time);
        //     VideoPlayerCtrl.instance.OnSeeking(seekValue);

        //     yield return new WaitForEndOfFrame();
        // }

        PlayCoroutine = null;

        // if (callback != null) callback();

        // /// UI
        // VideoPlayerCtrl.instance.OnPause();
    }
}
