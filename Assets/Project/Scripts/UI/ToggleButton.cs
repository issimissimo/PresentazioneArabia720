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
    public bool isOnAtStart = true;
    [HideInInspector]
    private bool isOn = true;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        imageOn = image.sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        isOn = isOnAtStart;
        button.onClick.AddListener(OnClicked);
        SetImage();
    }


    public void SetState(bool value){
        
        isOn = value;
        print("SETSTATE: " + value );
        SetImage();
    }


    public void Toggle(){
        isOn = !isOn;
        SetImage();
    }

    public bool IsOn(){
        return isOn;
    }



    void OnClicked()
    {
        isOn = !isOn;
        if (OnClick != null) OnClick.Invoke();
        SetImage();
    }


    public void SetImage()
    {
        print ("SET IMAGE " + isOn);
        image.sprite = isOn ? imageOn : ImageOff;
    }
}
