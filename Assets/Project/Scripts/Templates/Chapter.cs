using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chapter : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> childs;
    int childNumber = 0;
    Video video;
    Action onEndCallback;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            childs.Add(child);
            child.SetActive(false);
            // GameObject child = transform.GetChild(i).gameObject;
            // if (child.GetComponent<Video>() != null){
            //     videos.Add(child.GetComponent<Video>());
            // }
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }


    public void PlayAll(Action callback)
    {
        onEndCallback = callback;
        childNumber = 0;
        Play();
    }

    public void Play()
    {
        childs[childNumber].SetActive(true);

        if (childs[childNumber].GetComponent<Video>() != null)
        {
            video = childs[childNumber].GetComponent<Video>();
            video.Play(GoNextChild);
        }
    }

    public void GoNextChild()
    {
        if (video != null){
            video.Stop();
        }
        childs[childNumber].SetActive(false);

        childNumber++;
        if (childNumber <= childs.Count - 1)
        {
            Play();
        }
        else
        {
            onEndCallback();
        }
    }

    public void GoPreviousChild(Action onStartFoundcallback)
    {
        if (video != null){
            video.Stop();
        }
        childs[childNumber].SetActive(false);
        
        childNumber--;
        if (childNumber >= 0)
        {
            Play();
        }
        else
        {
            onStartFoundcallback();
        }
    }
}
