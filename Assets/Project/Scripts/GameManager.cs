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

    [HideInInspector]
    private int chapterNumber = -1;
    private int oldChapterNumber = -1;
    private int childNumber = -1;
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
        // PlayChapter(0);
        PlayChild(0, 0);
    }

    public void PlayChapter(int number, bool lastChild = false)
    {
        uiManager.UnselectPanel();
        uiManager.SelectPanel(number);

        /// Stop previous playing chapter (if any...)
        if (chapterNumber >= 0)
        {
            chapters[chapterNumber].Stop();
        }

        chapterNumber = number;
        chapters[chapterNumber].PlayAll(chapterNumber, lastChild, GoNextChapter);
    }


    public void PlayChild(int _chapterNumber, int _childNumber)
    {
        print("GAME MANAGER ----> PLAY CHILD");
        /// Stop previous playing chapter (if any...)
        if (oldChapterNumber >= 0 && _chapterNumber != oldChapterNumber && oldChildNumber >= 0)
        {
            print("CAMBIO DI CHAPTER! STOPPO QUELLO PRIMA...");
            chapters[oldChapterNumber].Stop();
        }

        oldChapterNumber = chapterNumber;
        oldChildNumber = childNumber;
        chapterNumber = _chapterNumber;
        childNumber = _childNumber;

        uiManager.UnselectChildPanel();
        uiManager.SelectChildPanel(chapterNumber, childNumber);
        chapters[chapterNumber].PlayChild(chapterNumber, childNumber, GoNextChapter);
    }


    public void GoNextChapterChild()
    {
        chapters[chapterNumber].GoNextChild(true);
    }

    public void GoPreviousChapterChild()
    {
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
        // if (playAuto)
        // {
        oldChapterNumber = chapterNumber;
        chapterNumber++;
        if (chapterNumber <= chapters.Count - 1)
        {
            // PlayChapter(chapterNumber);
            PlayChild(chapterNumber, 0);
        }
        else
        {
            chapterNumber--;
            print("END OF PRESENTATION!");
        }
        // }
    }

    private void GoPreviousChapter()
    {
        chapterNumber--;
        if (chapterNumber >= 0)
        {
            // PlayChapter(chapterNumber, true);
            int childNumber = chapters[chapterNumber].childs.Count - 1;
            PlayChild(chapterNumber, childNumber);
        }
        else
        {
            print("REACHED THE START OF PRESENTATION!");
        }
    }
}
