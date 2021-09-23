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

        //  if is first
        // if (chapterNumber == 0 && childNumber == 0)
        // {
        //     GameManager.instance.uiManager.previousButton.interactable = false;
        // }
        // else
        // {
        //     GameManager.instance.uiManager.previousButton.interactable = true;
        // }
        // if (chapterNumber == GameManager.instance.chapters.Count - 1 && childNumber == childs.Count - 1)
        // {
        //     GameManager.instance.uiManager.nextButton.interactable = false;
        // }
        // else
        // {
        //     GameManager.instance.uiManager.nextButton.interactable = true;
        // }


        childs[childNumber].SetActive(true);

        /// if is video...
        if (childs[childNumber].GetComponent<Video>() != null)
        {
            video = childs[childNumber].GetComponent<Video>();
            video.Play(GoNextChild);
        }
    }


    public void Stop()
    {
        if (video != null)
        {
            video.Stop();
            video = null;

            print("Stop: " + gameObject.name + " - " + childNumber);
            childs[childNumber].SetActive(false);
        }

    }


    public void GoNextChild()
    {
        // if (video != null){
        //     video.Stop();
        // }
        // childs[childNumber].SetActive(false);

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
        // if (video != null){
        //     video.Stop();
        // }
        // childs[childNumber].SetActive(false);

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
