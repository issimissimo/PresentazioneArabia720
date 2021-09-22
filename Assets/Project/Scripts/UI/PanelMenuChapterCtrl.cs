using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using UnityEngine.EventSystems;

public class PanelMenuChapterCtrl : MonoBehaviour
{
    public TextMeshProUGUI englishTitle;

    [HideInInspector]
    public string prefabName;
    private bool isOver;

    public void Setup(Chapter chapter)
    {
        prefabName = chapter.gameObject.name;
        englishTitle.text = prefabName;
        gameObject.name = prefabName;
    }

    public void MouseOver()
    {
        print("MOUSE OVER: " + prefabName);
    }

    public void MouseExit()
    {
        print("MOUSE EXIT: " + prefabName);
    }



}
