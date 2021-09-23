using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using System.IO;

public class Video : MonoBehaviour
{
    public string videoName;
    public string speakerAudioName;
    VideoPlayer videoPlayer;
    Coroutine PlayCoroutine;
    Action onVideoFinishedCallback;
    bool isPlaying;
    AudioClip audioClip;
    AudioSource audioSource;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        VideoPlayerCtrl.instance.OnPLayEvent += OnPlayFromUI;
        VideoPlayerCtrl.instance.OnPauseEvent += OnPauseFromUI;
        VideoPlayerCtrl.instance.OnStartSeekEvent += OnStartSeekFromUI;
        VideoPlayerCtrl.instance.OnSeekEvent += OnSeekFromUI;
        VideoPlayerCtrl.instance.OnEndSeekEvent += OnEndSeekFromUI;
        videoPlayer.loopPointReached += OnVideoFinished;
        GameManager.OnSpeakerToggleEvent += OnSpeakerToggle;
    }


    private void OnDisable()
    {
        VideoPlayerCtrl.instance.OnPLayEvent -= OnPlayFromUI;
        VideoPlayerCtrl.instance.OnPauseEvent -= OnPauseFromUI;
        VideoPlayerCtrl.instance.OnStartSeekEvent -= OnStartSeekFromUI;
        VideoPlayerCtrl.instance.OnSeekEvent -= OnSeekFromUI;
        VideoPlayerCtrl.instance.OnEndSeekEvent -= OnEndSeekFromUI;
        videoPlayer.loopPointReached -= OnVideoFinished;
        GameManager.OnSpeakerToggleEvent -= OnSpeakerToggle;
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

    private IEnumerator GetAudioClip(Action callback)
    {
        WWW request = new WWW(Path.Combine(Application.streamingAssetsPath, speakerAudioName));
        yield return request;
        audioClip = request.GetAudioClip();
        audioSource.clip = audioClip;
        request.Dispose();
        callback();
    }


    private void PlayAudioClip()
    {
        if (audioClip == null)
        {
            StartCoroutine(GetAudioClip(() =>
            {
                audioSource.Play();
            }));
        }
        else
        {
            audioSource.Play();
        }
    }


    private void OnSpeakerToggle(bool value)
    {
        print(gameObject.name + " - OnSpeakerToggle: " + value);
        if (value)
        {
            audioSource.mute = false;
        }
        else
        {
            audioSource.mute = true;
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
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
        OnSpeakerToggle(GameManager.instance.speakerIsOn);

        if (!string.IsNullOrEmpty(speakerAudioName))
        {
            PlayAudioClip();
        }

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
        videoPlayer.url = Path.Combine(Application.streamingAssetsPath, videoName);

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

        videoPlayer.frame = 0;
        videoPlayer.Play();

        PlayCoroutine = null;
    }
}
