using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public Transform chaptersContainer;
    public CanvasFader blackPanel;

    [HideInInspector]
    public List<Chapter> chapters;
    public UiManager uiManager;


    public static int chapterNumber = 0;
    private int oldChapterNumber = -1;
    public static int childNumber = 0;
    private int oldChildNumber = -1;


    public static GameManager instance;

    [HideInInspector]
    public bool speakerIsOn;
    [HideInInspector]
    public bool playAuto;

    public static event Action<bool> OnSpeakerToggleEvent;
    public static event Action<bool> OnPlayAutoToggleEvent;


    private void Awake()
    {
        instance = this;

        for (int i = 0; i < chaptersContainer.childCount; ++i)
        {
            GameObject child = chaptersContainer.GetChild(i).gameObject;
            if (child.GetComponent<Chapter>() != null)
            {
                chapters.Add(child.GetComponent<Chapter>());
            }
        }

        speakerIsOn = uiManager.speaker.isOn;
        playAuto = uiManager.auto.isOn;
    }


    void Start()
    {
        /// keep cursor in the window
        // Cursor.lockState = CursorLockMode.Confined;

        uiManager.SetupMenu(chapters);
        PlayAll();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GoNextChapterChild();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GoPreviousChapterChild();
        }
    }


    public void PlayAll()
    {
        PlayChild(0, 0);
    }


    public void PlayChild(int _chapterNumber, int _childNumber)
    {
        uiManager.UnselectChildPanel();
        uiManager.SelectChildPanel(_chapterNumber, _childNumber);

        // oldChapterNumber = chapterNumber;
        // chapterNumber = _chapterNumber;

        // oldChildNumber = childNumber;
        // childNumber = _childNumber;

        print("PLAY CHILD ---> oldChapter: " + oldChapterNumber + " - newChapter: " + _chapterNumber + " - oldChild: " + oldChildNumber + " - newChild: " + _childNumber);

        // int chapterToStop = oldChapterNumber;
        // int childrenToStop = oldChildNumber;

        // if (_chapterNumber != oldChapterNumber && oldChapterNumber >= 0)
        //     chapterToStop = oldChapterNumber;
        // else if (_chapterNumber == oldChapterNumber)
        //     chapterToStop = _chapterNumber;

        // if (_childNumber != oldChildNumber && oldChildNumber >= 0)
        //     childrenToStop = oldChildNumber;
        // else if (_childNumber == oldChildNumber)
        //     childrenToStop = _childNumber;

        // if (chapterToStop >= 0 && childrenToStop >= 0)
        // {

        // BlackPanel.instance.FadeIn(() =>
        // {
        //     chapters[chapterToStop].StopChild(childrenToStop);
        //     PlayChild();
        // });


        if (BlackPanel.isFading)
        {
            ///////////////////////////////////////////////
            ////////////////////////////////////////
            ///////////////////////
            print("IS FADING");

            oldChapterNumber = chapterNumber;
            oldChildNumber = childNumber;

            chapterNumber = _chapterNumber;
            childNumber = _childNumber;


            if (oldChapterNumber >= 0 && oldChildNumber >= 0)
                chapters[oldChapterNumber].StopChild(oldChildNumber);

            PlayChild();
        }
        else
        {
            // chapters[chapterToStop].StopChild(childrenToStop);
            // PlayChild();
            // print("IS NOT FADING");
            BlackPanel.instance.FadeIn(() =>
            {

                oldChapterNumber = chapterNumber;
                oldChildNumber = childNumber;

                chapterNumber = _chapterNumber;
                childNumber = _childNumber;

                print("---> oldChapter: " + oldChapterNumber + " - newChapter: " + chapterNumber + " - oldChild: " + oldChildNumber + " - newChild: " + childNumber);

                if (oldChapterNumber >= 0 && oldChildNumber >= 0)
                    chapters[oldChapterNumber].StopChild(oldChildNumber);

                PlayChild();
            });
            // }


            // chapters[chapterToStop].StopChild(childrenToStop);
            // PlayChild();
        }
        // else
        // {
        //     print("NON STOPPO");
        //     oldChapterNumber = chapterNumber;
        //     chapterNumber = _chapterNumber;

        //     oldChildNumber = childNumber;
        //     childNumber = _childNumber;


        //     PlayChild();
        // }
    }

    private void PlayChild()
    {
        BlackPanel.instance.FadeOut();

        chapters[chapterNumber].PlayChild(chapterNumber, childNumber, GoNextChapter);
    }


    public void GoNextChapterChild()
    {
        // if (BlackPanel.isFading) return;
        chapters[chapterNumber].GoNextChild(true);
    }

    public void GoPreviousChapterChild()
    {
        // if (BlackPanel.isFading) return;
        chapters[chapterNumber].GoPreviousChild(GoPreviousChapter);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleSpeaker()
    {
        speakerIsOn = !speakerIsOn;
        if (OnSpeakerToggleEvent != null) OnSpeakerToggleEvent(speakerIsOn);
    }

    public void TogglePlayAuto()
    {
        playAuto = !playAuto;
        if (OnPlayAutoToggleEvent != null) OnPlayAutoToggleEvent(playAuto);
    }


    private void GoNextChapter()
    {
        if (chapterNumber + 1 <= chapters.Count - 1)
        {
            PlayChild(chapterNumber + 1, 0);
        }
        else
        {
            print("REACHED THE END OF PRESENTATION!");
        }
    }

    private void GoPreviousChapter()
    {
        if (chapterNumber - 1 >= 0)
        {
            int childNumber = chapters[chapterNumber - 1].childs.Count - 1;
            PlayChild(chapterNumber - 1, childNumber);
        }
        else
        {
            print("REACHED THE START OF PRESENTATION!");
        }
    }
}
