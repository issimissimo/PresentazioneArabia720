﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Transform menuContainer;
    public GameObject chapterMenuPrefab;
    public GameObject childMenuPrefab;
    public Button previousButton;
    public Button nextButton;
    public ToggleButton speaker;
    public ToggleButton auto;

    public GameObject videoPlayerControls;

    string uiElementName;
    string panelSelectedName;
    private PanelMenuChapterCtrl panel;

    List<PanelMenuChapterCtrl> panelMenuChapterCtrls = new List<PanelMenuChapterCtrl>();
    List<PanelMenuChildrCtrl> allChilds = new List<PanelMenuChildrCtrl>();


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
            panel = GetPanelByName(uiElementName);
            panel.PlayChapter();

            /// close menu
            SideMenuCtrl.instance.Toggle();
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
            print(chapter.childs.Count);

            GameObject go = Instantiate(chapterMenuPrefab, menuContainer);
            PanelMenuChapterCtrl _panelMenuChapterCtrl = go.GetComponent<PanelMenuChapterCtrl>();
            _panelMenuChapterCtrl.Setup(chapter, i);
            panelMenuChapterCtrls.Add(_panelMenuChapterCtrl);

            /// create sub items
            // List<GameObject> childs = new List<GameObject>();
            int ii = 0;
            foreach (GameObject child in chapter.childs)
            {
                GameObject newGo = Instantiate(childMenuPrefab, menuContainer);
                PanelMenuChildrCtrl _panelMenuChildCtrl = newGo.GetComponent<PanelMenuChildrCtrl>();
                _panelMenuChildCtrl.Setup(i, ii, child);
                // allChilds.Add(_panelMenuChildCtrl);

                ii++;
            }
            i++;
        }
    }
}
