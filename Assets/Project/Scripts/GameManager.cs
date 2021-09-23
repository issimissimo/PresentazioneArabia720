using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        uiManager.SetupMenu(chapters);
        PlayAll();
    }


    public void PlayAll()
    {
        PlayChapter(0);
    }

    public void PlayChapter(int number)
    {
        Debug.Log("PLAYCHAPTER: " + number);
        /// Stop previous playing chapter (if any...)
        if (chapterNumber >= 0)
        {
            chapters[chapterNumber].Stop();
        }

        chapterNumber = number;
        chapters[chapterNumber].PlayAll(GoNextChapter);
    }

    public void GoNextChapterChild()
    {
        chapters[chapterNumber].GoNextChild();
    }

    public void GoPreviousChapterChild()
    {
        chapters[chapterNumber].GoPreviousChild(GoPreviousChapter);
    }

    public void Quit()
    {
        Application.Quit();
    }


    private void GoNextChapter()
    {
        chapterNumber++;
        if (chapterNumber <= chapters.Count - 1)
        {
            PlayChapter(chapterNumber);
        }
        else
        {
            print("END OF PRESENTATION!");
        }
    }

    private void GoPreviousChapter()
    {
        chapterNumber--;
        if (chapterNumber >= 0)
        {
            PlayChapter(chapterNumber);
        }
        else
        {
            print("REACHED THE START OF PRESENTATION!");
        }
    }
}
