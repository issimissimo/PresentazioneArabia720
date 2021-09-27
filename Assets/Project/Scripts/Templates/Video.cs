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
    public bool loop;
    VideoPlayer videoPlayer;
    Coroutine PlayCoroutine;
    Action onVideoFinishedCallback;
    bool isPlaying;
    bool isLooping;
    AudioClip audioClip;
    AudioSource audioSource;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
        OnSetLoopToggle(loop);
    }

    private void Start() {
        // OnSetLoopToggle(loop);
        // VideoPlayerCtrl.instance.loopToggleButton.SetState(loop);
    }

    private void OnEnable()
    {
        VideoPlayerCtrl.OnPLayEvent += OnPlayFromUI;
        VideoPlayerCtrl.OnPauseEvent += OnPauseFromUI;
        VideoPlayerCtrl.OnStartSeekEvent += OnStartSeekFromUI;
        VideoPlayerCtrl.OnSeekEvent += OnSeekFromUI;
        VideoPlayerCtrl.OnEndSeekEvent += OnEndSeekFromUI;
        // VideoPlayerCtrl.OnSetLoopEvent += OnSetLoopToggle;
        videoPlayer.loopPointReached += OnVideoFinished;
        GameManager.OnSpeakerToggleEvent += OnSpeakerToggle;

        // OnSetLoopToggle(loop);
        // VideoPlayerCtrl.instance.loopToggleButton.SetState(loop);
    }


    private void OnDisable()
    {
        VideoPlayerCtrl.OnPLayEvent -= OnPlayFromUI;
        VideoPlayerCtrl.OnPauseEvent -= OnPauseFromUI;
        VideoPlayerCtrl.OnStartSeekEvent -= OnStartSeekFromUI;
        VideoPlayerCtrl.OnSeekEvent -= OnSeekFromUI;
        VideoPlayerCtrl.OnEndSeekEvent -= OnEndSeekFromUI;
        // VideoPlayerCtrl.OnSetLoopEvent -= OnSetLoopToggle;
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


    private void LoadAudioClip()
    {
        if (audioClip == null)
        {
            StartCoroutine(GetAudioClip(() =>
            {
                PlayAudioClip();
            }));
        }
        else
        {
            PlayAudioClip();
        }
    }


    private void PlayAudioClip()
    {
        audioSource.time = 0;
        audioSource.Play();
    }


    private void OnSpeakerToggle(bool value)
    {
        // print(gameObject.name + " - OnSpeakerToggle: " + value);
        if (value)
        {
            audioSource.mute = false;
        }
        else
        {
            audioSource.mute = true;
        }
    }


    public void OnSetLoopToggle(bool value)
    {
        isLooping = value;
        videoPlayer.isLooping = value;
        print("VIDEO ---> looping: " + videoPlayer.isLooping);
    }


    private void OnVideoFinished(VideoPlayer vp)
    {
        if (!videoPlayer.isLooping)
        {
            isPlaying = false;
            VideoPlayerCtrl.instance.OnPause();
            if (onVideoFinishedCallback != null) onVideoFinishedCallback();
        }
    }

    private void OnPlayFromUI()
    {
        videoPlayer.Play();
        audioSource.Play();
    }

    private void OnPauseFromUI()
    {
        videoPlayer.Pause();
        audioSource.Pause();
    }

    private void OnStartSeekFromUI()
    {
        isPlaying = videoPlayer.isPlaying ? true : false;
        if (isPlaying) OnPauseFromUI();
    }

    private void OnSeekFromUI(float val)
    {
        /// videoplayer seek
        float time = Mathf.Lerp(0, (float)videoPlayer.length, val);
        videoPlayer.time = time;

        /// audiosource seek
        audioSource.time = Mathf.Lerp(0, (float)videoPlayer.length, val);
    }

    private void OnEndSeekFromUI()
    {
        /// qui dobbiamo aspettare un attimo,
        /// se no abbiamo lo scatto brutto dello slider...
        if (isPlaying) StartCoroutine(OnEndSeekFromUICoroutine());
    }

    IEnumerator OnEndSeekFromUICoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        OnPlayFromUI();
    }


    public void Play(Action callback)
    {
        OnSpeakerToggle(GameManager.instance.speakerIsOn);

        if (!string.IsNullOrEmpty(speakerAudioName))
        {
            LoadAudioClip();
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
        VideoPlayerCtrl.instance.loopToggleButton.SetState(isLooping);

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
