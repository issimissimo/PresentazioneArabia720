using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UiManager : MonoBehaviour
{
    public Transform menuContainer;
    public GameObject chapterMenuPrefab;
    public Button previousButton;
    public Button nextButton;

    public GameObject videoPlayerControls;

    string uiElementName;
    string panelSelectedName;
    private PanelMenuChapterCtrl panel;

    List<PanelMenuChapterCtrl> panelMenuChapterCtrls = new List<PanelMenuChapterCtrl>();

    // public static event Action pp;



    PanelMenuChapterCtrl GetPanelByName(string name)
    {
        PanelMenuChapterCtrl _panel = null;
        foreach (PanelMenuChapterCtrl __panel in panelMenuChapterCtrls)
        {
            if (__panel.prefabName == name)
            {
                _panel = __panel;
            }
        }
        return _panel;
    }

    PanelMenuChapterCtrl GetPanelByNumber(int number)
    {
        return panelMenuChapterCtrls[number];
    }



    void Update()
    {
        string name = null;
        if (Utils.IsPointerOverUI("Menu", out name))
        {
            if (name != uiElementName)
            {
                uiElementName = name;
                panel = GetPanelByName(uiElementName);
                panel.SetHighlight();
            }
        }
        else
        {
            if (uiElementName != null)
            {
                panel = GetPanelByName(uiElementName);
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
            panel = GetPanelByName(uiElementName);
            panel.PlayChapter();
        }
    }


    public void SelectPanel(int number)
    {
        panel = GetPanelByNumber(number);
        panel.SetSelected();
        panelSelectedName = panel.prefabName;
    }

    public void UnselectPanel()
    {
        if (panelSelectedName != null)
        {
            panel = GetPanelByName(panelSelectedName);
            panel.SetUnselected();
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
