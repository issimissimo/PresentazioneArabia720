using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PanelMenuChapterCtrl : MonoBehaviour
{
    public TextMeshProUGUI englishTitle;

    [HideInInspector]
    public string prefabName;
    private int prefabNumber;
    // private bool isOver;
    private bool isClicked;
    public Image backgroundImage;

    private Color defaultColor;
    public Color overColor;
    public Color clickColor;

    private void Awake()
    {
        // backgroundImage = GetComponent<Image>();
        defaultColor = backgroundImage.color;
    }

    public void Setup(Chapter chapter, int number)
    {
        prefabName = chapter.gameObject.name;
        englishTitle.text = prefabName;
        gameObject.name = prefabName;
        prefabNumber = number;
    }

    public void SetHighlight()
    {
        if (!isClicked)
        {
            // print("MOUSE OVER: " + prefabName);
            backgroundImage.color = overColor;
        }
    }

    public void SetDefault()
    {
        if (!isClicked)
        {
            // print("MOUSE EXIT: " + prefabName);
            backgroundImage.color = defaultColor;
        }
    }

    public void SetSelected()
    {
        if (!isClicked)
        {
            isClicked = true;
            // print("MOUSE CLICK: " + prefabName + " - " + prefabNumber);
            backgroundImage.color = clickColor;
        }
    }

    public void SetUnselected()
    {
        if (isClicked)
        {
            isClicked = false;
            SetDefault();
        }
    }

    public void PlayChapter()
    {
        // GameManager.instance.PlayChapter(prefabNumber);
    }


}
