using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public Transform menuContainer;
    public GameObject chapterMenuPrefab;

    string uiElementName;

    List<PanelMenuChapterCtrl> panelMenuChapterCtrls = new List<PanelMenuChapterCtrl>();

    void Start()
    {

    }

    void Update()
    {
        string name = null;
        if (Utils.IsPointerOverUI("Menu", out name))
        {
            if (name != uiElementName)
            {
                uiElementName = name;
                // Debug.Log("Its entering the UI elements named: " + uiElementName);

                foreach (PanelMenuChapterCtrl panel in panelMenuChapterCtrls)
                {
                    if (panel.prefabName == uiElementName)
                    {
                        panel.MouseOver();
                    }
                }
            }
        }
        else
        {
            if (uiElementName != null)
            {
                // Debug.Log("Its exiting the UI elements named: " + uiElementName);
                foreach (PanelMenuChapterCtrl panel in panelMenuChapterCtrls)
                {
                    if (panel.prefabName == uiElementName)
                    {
                        panel.MouseExit();
                    }
                }
                uiElementName = null;
            }
        }
    }



    public void SetupMenu(List<Chapter> chapters)
    {
        foreach (Chapter chapter in chapters)
        {
            GameObject go = Instantiate(chapterMenuPrefab, menuContainer);
            PanelMenuChapterCtrl _panelMenuChapterCtrl = go.GetComponent<PanelMenuChapterCtrl>();
            _panelMenuChapterCtrl.Setup(chapter);
            panelMenuChapterCtrls.Add(_panelMenuChapterCtrl);
        }
    }
}
