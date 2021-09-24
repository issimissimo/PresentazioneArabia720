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

    int chapterNumber;
    int childNumber;
    GameObject gameObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(int _chapterNumber, int _childNumber, GameObject _gameObject){
        print("Setup");
        chapterNumber = _chapterNumber;
        childNumber = _childNumber;
        gameObject = _gameObject;

        englishTitle.text = gameObject.name;
        
        if (gameObject.GetComponent<Video>() != null){
            icon.sprite = iconVideo;
        }
        if (gameObject.GetComponent<Picture>() != null){
            icon.sprite = iconPicture;
        }
    }
}
