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
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        // videoPlayer.url = fileUrl;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForEndOfFrame();
        }

        videoPlayer.frame = 0;
        videoPlayer.Play();

        // yield return new WaitForSeconds(0.1f);



        while (videoPlayer.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        PlayCoroutine = null;

        if (callback != null) callback();
    }


}
