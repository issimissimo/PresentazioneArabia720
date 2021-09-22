using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform chaptersContainer;
    public CanvasFader blackPanel;
    public List<Chapter> chapters;
    int chapterNumber = 0;
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
        PlayAll();
    }


    public void PlayAll()
    {
        chapterNumber = 0;
        PlayChapter();
    }

    public void PlayChapter()
    {
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
            PlayChapter();
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
            PlayChapter();
        }
        else
        {
            print("REACHED THE START OF PRESENTATION!");
        }
    }
}
