using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chapter : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> childs;
    int chapterNumber;
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
        }
    }



    public void PlayAll(int number, bool lastChild, Action callback)
    {
        chapterNumber = number;
        onEndCallback = callback;
        childNumber = lastChild ? childs.Count - 1 : 0;
        PlayChild();
    }

    public void PlayChild()
    {
        /// set UI buttons
        GameManager.instance.uiManager.previousButton.interactable =
            chapterNumber == 0 && childNumber == 0 ? false : true;

        GameManager.instance.uiManager.nextButton.interactable =
            chapterNumber == GameManager.instance.chapters.Count - 1 && childNumber == childs.Count - 1 ? false : true;

        childs[childNumber].SetActive(true);

        /// if the child is video...
        if (childs[childNumber].GetComponent<Video>() != null)
        {
            GameManager.instance.uiManager.videoPlayerControls.SetActive(true);

            video = childs[childNumber].GetComponent<Video>();
            video.Play(GoNextChild);
        }
        /// if not video...
        else
        {
            GameManager.instance.uiManager.videoPlayerControls.SetActive(false);
        }
    }


    public void Stop()
    {
        if (video != null)
        {
            video.Stop();
            video = null;

            // print("Stop: " + gameObject.name + " - " + childNumber);
            childs[childNumber].SetActive(false);
        }

    }


    public void GoNextChild()
    {
        Stop();

        childNumber++;
        if (childNumber <= childs.Count - 1)
        {
            PlayChild();
        }
        else
        {
            onEndCallback();
        }
    }

    public void GoPreviousChild(Action onStartFoundcallback)
    {
        Stop();

        childNumber--;
        if (childNumber >= 0)
        {
            PlayChild();
        }
        else
        {
            onStartFoundcallback();
        }
    }
}
