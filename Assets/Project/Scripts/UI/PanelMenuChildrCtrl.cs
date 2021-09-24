using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelMenuChildrCtrl : MonoBehaviour
{
    public TextMeshProUGUI englishTitle;
    public Image icon;
    public Sprite iconVideo;
    public Sprite iconPicture;

    private bool isClicked;

    int chapterNumber;
    int childNumber;
    GameObject gameObject;

    private Color defaultColor;
    public Color overColor;
    public Color clickColor;
    private Image backgroundImage;


    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        defaultColor = backgroundImage.color;
    }

 
    public void Setup(int _chapterNumber, int _childNumber, GameObject _gameObject)
    {
        chapterNumber = _chapterNumber;
        childNumber = _childNumber;
        gameObject = _gameObject;

        englishTitle.text = gameObject.name;

        if (gameObject.GetComponent<Video>() != null)
        {
            icon.sprite = iconVideo;
        }
        if (gameObject.GetComponent<Picture>() != null)
        {
            icon.sprite = iconPicture;
        }
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
            print("MOUSE EXIT: " + gameObject.name);
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

    public void PlayChild()
    {
        GameManager.instance.PlayChild(chapterNumber, childNumber);
    }
}
