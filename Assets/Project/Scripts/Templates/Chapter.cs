using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chapter : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> childs;
    int chapterNumber;
    int childNumber;
    int oldChildNumber;
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



    public void PlayAll(int _chapterNumber, bool lastChild, Action callback)
    {
        chapterNumber = _chapterNumber;
        onEndCallback = callback;
        childNumber = lastChild ? childs.Count - 1 : 0;
        PlayChild();
    }


    public void PlayChild(int _chapterNumber, int _childNumber, Action callback)
    {
        chapterNumber = _chapterNumber;
        oldChildNumber = childNumber;
        childNumber = _childNumber;
        onEndCallback = callback;

        // print("PLAYCHILD " + childNumber + " - OLD: " + oldChildNumber);


        BlackPanel.instance.FadeIn(() =>
        {
            Stop();


            PlayChild();

        });

        // PlayChild();
    }



    private void PlayChild()
    {
        /// set UI buttons
        GameManager.instance.uiManager.previousButton.interactable =
            chapterNumber == 0 && childNumber == 0 ? false : true;

        GameManager.instance.uiManager.nextButton.interactable =
            chapterNumber == GameManager.instance.chapters.Count - 1 && childNumber == childs.Count - 1 ? false : true;


        childs[childNumber].SetActive(true);

        /// black panel
        BlackPanel.instance.FadeOut();

        /// if the child is VIDEO...
        if (childs[childNumber].GetComponent<Video>() != null)
        {
            GameManager.instance.uiManager.videoPlayerControls.SetActive(true);

            video = childs[childNumber].GetComponent<Video>();
            video.Play(() =>
            {
                GoNextChild();
            });
        }
        /// if the child is PICTURE...
        else if (childs[childNumber].GetComponent<Picture>() != null)
        {
            GameManager.instance.uiManager.videoPlayerControls.SetActive(false);

            picture = childs[childNumber].GetComponent<Picture>();
            picture.Play(() =>
            {
                GoNextChild();
            });
        }
    }



    public void Stop()
    {
        if (video != null)
        {
            print(gameObject.name +  " --> STOP CHILD: " + oldChildNumber);
            video.Stop();
            video = null;
            childs[oldChildNumber].SetActive(false);
        }
        else if (picture != null)
        {
            print(gameObject.name +  " --> STOP CHILD: " + oldChildNumber);
            picture = null;
            childs[oldChildNumber].SetActive(false);
        }
    }



    public void GoNextChild(bool forced = false)
    {
        if (chapterNumber == GameManager.instance.chapters.Count - 1 && childNumber == childs.Count - 1) return;

        if (!GameManager.instance.playAuto && !forced) return;

        // BlackPanel.instance.FadeIn(() =>
        // {
        //     Stop();

        //     childNumber++;
        //     if (childNumber <= childs.Count - 1)
        //     {
        //         // PlayChild();
        //         /// we have to call the function from GameManager
        //         /// to highlight the menu panels...
        //         GameManager.instance.PlayChild(chapterNumber, childNumber);
        //     }
        //     else
        //     {
        //         // print("onEndCallback");
        //         onEndCallback();
        //     }
        // });
        if (childNumber + 1 <= childs.Count - 1)
        {
            GameManager.instance.PlayChild(chapterNumber, childNumber + 1);
        }
        else
        {
            print("FINITO IL CHAPTER, PASSO A QUELLO SUCCESSIVO...");
            oldChildNumber ++;
            onEndCallback();
        }
    }



    public void GoPreviousChild(Action onStartFoundcallback)
    {
        if (chapterNumber == 0 && childNumber == 0) return;

        // BlackPanel.instance.FadeIn(() =>
        // {
        //     Stop();

        //     childNumber--;
        //     if (childNumber >= 0)
        //     {
        //         // PlayChild();
        //         /// we have to call the function from GameManager
        //         /// to highlight the menu panels...
        //         GameManager.instance.PlayChild(chapterNumber, childNumber);
        //     }
        //     else
        //     {
        //         onStartFoundcallback();
        //     }
        // });
        if (childNumber - 1 >= 0)
        {
            GameManager.instance.PlayChild(chapterNumber, childNumber - 1);
        }
        else
        {
            oldChildNumber --;
            onStartFoundcallback();
        }
    }
}
