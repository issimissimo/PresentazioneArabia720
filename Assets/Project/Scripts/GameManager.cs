﻿using System.Collections;
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
    public static GameManager instance;

    [HideInInspector]
    public bool speakerIsOn = true;
    [HideInInspector]
    public bool playAuto = true;

    public static event Action<bool> OnSpeakerToggleEvent;


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
    }


    void Start()
    {
        /// keep cursor in the window
        Cursor.lockState = CursorLockMode.Confined;

        uiManager.SetupMenu(chapters);
        PlayAll();
    }


    public void PlayAll()
    {
        PlayChapter(0);
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
    }


    private void GoNextChapter()
    {
        // if (playAuto)
        // {
        chapterNumber++;
        if (chapterNumber <= chapters.Count - 1)
        {
            PlayChapter(chapterNumber);
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
            PlayChapter(chapterNumber, true);
        }
        else
        {
            print("REACHED THE START OF PRESENTATION!");
        }
    }
}
