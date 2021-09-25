using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chapter : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> childs;
    int chapterNumber;
    // int childNumber;
    Video video;
    Picture picture;
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



    // public void PlayAll(int _chapterNumber, bool lastChild, Action callback)
    // {
    //     chapterNumber = _chapterNumber;
    //     onEndCallback = callback;
    //     childNumber = lastChild ? childs.Count - 1 : 0;
    //     PlayChild();
    // }


    public void PlayChild(int _chapterNumber, int _childNumber, Action callback)
    {
        chapterNumber = _chapterNumber;
        // childNumber = _childNumber;
        onEndCallback = callback;

        PlayChild();
    }



    private void PlayChild()
    {
        /// set UI buttons
        GameManager.instance.uiManager.previousButton.interactable =
            chapterNumber == 0 && GameManager.childNumber == 0 ? false : true;

        GameManager.instance.uiManager.nextButton.interactable =
            chapterNumber == GameManager.instance.chapters.Count - 1 && GameManager.childNumber == childs.Count - 1 ? false : true;


        childs[GameManager.childNumber].SetActive(true);


        /// if the child is VIDEO...
        if (childs[GameManager.childNumber].GetComponent<Video>() != null)
        {
            GameManager.instance.uiManager.videoPlayerControls.SetActive(true);

            video = childs[GameManager.childNumber].GetComponent<Video>();
            video.Play(() =>
            {
                GoNextChild();
            });
        }
        /// if the child is PICTURE...
        else if (childs[GameManager.childNumber].GetComponent<Picture>() != null)
        {
            GameManager.instance.uiManager.videoPlayerControls.SetActive(false);

            picture = childs[GameManager.childNumber].GetComponent<Picture>();
            picture.Play(() =>
            {
                GoNextChild();
            });
        }
    }



    public void StopChild(int _childNumber)
    {
        if (video != null)
        {
            print(gameObject.name + " --> STOP CHILD: " + _childNumber);
            video.Stop();
            video = null;
            childs[_childNumber].SetActive(false);
        }
        else if (picture != null)
        {
            print(gameObject.name + " --> STOP CHILD: " + _childNumber);
            picture = null;
            childs[_childNumber].SetActive(false);
        }
    }



    public void GoNextChild(bool forced = false)
    {
        if (chapterNumber == GameManager.instance.chapters.Count - 1 && GameManager.childNumber == childs.Count - 1) return;

        if (!GameManager.instance.playAuto && !forced) return;

        
        if (GameManager.childNumber + 1 <= childs.Count - 1)
        {
            GameManager.instance.PlayChild(chapterNumber, GameManager.childNumber + 1);
        }
        else
        {
            print("FINITO IL CHAPTER, PASSO A QUELLO SUCCESSIVO...");
            onEndCallback();
        }
    }



    public void GoPreviousChild(Action onStartFoundcallback)
    {
        if (chapterNumber == 0 && GameManager.childNumber == 0) return;

        if (GameManager.childNumber - 1 >= 0)
        {
            GameManager.instance.PlayChild(chapterNumber, GameManager.childNumber - 1);
        }
        else
        {
            print("INIZIO DEL CHAPTER, PASSO A QUELLO PRECEDENTE...");
            onStartFoundcallback();
        }
    }
}
