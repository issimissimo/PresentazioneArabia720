using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class ToggleButton : MonoBehaviour
{
    private Sprite imageOn;
    public Sprite ImageOff;

    [Serializable]
    public class OnClickEvent : UnityEvent { }
    public OnClickEvent OnClick;

    Button button;
    Image image;
    bool isOn = true;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        imageOn = image.sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnClicked);
        SetImage();
    }



    void OnClicked()
    {
        isOn = !isOn;
        if (OnClick != null) OnClick.Invoke();
        SetImage();
    }


    void SetImage()
    {
        image.sprite = isOn ? imageOn : ImageOff;
    }
}
