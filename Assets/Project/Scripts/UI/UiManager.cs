using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UiManager : MonoBehaviour
{
    public Transform menuContainer;
    public GameObject chapterMenuPrefab;

    string uiElementName;
    private PanelMenuChapterCtrl panel;

    List<PanelMenuChapterCtrl> panelMenuChapterCtrls = new List<PanelMenuChapterCtrl>();

    // public static event Action pp;

    void Start()
    {

    }



    PanelMenuChapterCtrl GetPanel()
    {
        PanelMenuChapterCtrl _panel = null;
        foreach (PanelMenuChapterCtrl __panel in panelMenuChapterCtrls)
        {
            if (__panel.prefabName == uiElementName)
            {
                _panel = __panel;
            }
        }
        return _panel;
    }



    void Update()
    {
        string name = null;
        if (Utils.IsPointerOverUI("Menu", out name))
        {
            if (name != uiElementName)
            {
                /// turn On the new one
                uiElementName = name;
                // foreach (PanelMenuChapterCtrl panel in panelMenuChapterCtrls)
                // {
                //     if (panel.prefabName == uiElementName)
                //     {
                //         panel.SetHighlight();
                //     }
                // }
                panel = GetPanel();
                panel.SetHighlight();
            }
        }
        else
        {
            if (uiElementName != null)
            {
                // foreach (PanelMenuChapterCtrl panel in panelMenuChapterCtrls)
                // {
                //     if (panel.prefabName == uiElementName)
                //     {
                //         panel.SetDefault();
                //     }
                // }
                panel = GetPanel();
                panel.SetDefault();
                uiElementName = null;
            }
        }

        if (Input.GetMouseButtonDown(0) && uiElementName != null)
        {
            // foreach (PanelMenuChapterCtrl panel in panelMenuChapterCtrls)
            // {
            //     if (panel.prefabName == uiElementName)
            //     {
            //         // panel.SetSelected(() =>
            //         // {
            //         //     panel.PlayChapter();
            //         // });
            //         panel.PlayChapter();
            //     }
            // }
            panel = GetPanel();
            panel.PlayChapter();
        }
    }



    public void SetupMenu(List<Chapter> chapters)
    {
        int i = 0;
        foreach (Chapter chapter in chapters)
        {
            GameObject go = Instantiate(chapterMenuPrefab, menuContainer);
            PanelMenuChapterCtrl _panelMenuChapterCtrl = go.GetComponent<PanelMenuChapterCtrl>();
            _panelMenuChapterCtrl.Setup(chapter, i);
            panelMenuChapterCtrls.Add(_panelMenuChapterCtrl);
            i++;
        }
    }
}
