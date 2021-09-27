using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public ScrollRect scrollViewport;
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
    private bool menuPanelHasBeenClicked;
    bool justStarted = true;

    GameObject uiGameobject;
    GameObject uiGameobjectSelected;


    List<PanelMenuChapterCtrl> panelMenuChapterCtrls = new List<PanelMenuChapterCtrl>();
    List<List<GameObject>> allChilds = new List<List<GameObject>>();


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



    public void ScrollToCurrentElement()
    {
        int siblingIndex = uiGameobjectSelected.transform.GetSiblingIndex();

        /// correct the problem that the 1st chapter
        /// that has heigth different from the buttons, is cutted on top...
        if (siblingIndex == 1)
        {
            scrollViewport.normalizedPosition = new Vector2(0, 1);
        }
        else
        {
            float pos = 1f - (float)siblingIndex / scrollViewport.content.transform.childCount;

            if (pos < 0.4)
            {
                float correction = 1f / scrollViewport.content.transform.childCount;
                pos -= correction;
            }

            scrollViewport.verticalNormalizedPosition = pos;
        }
    }


    void Update()
    {
        // string name = null;
        // if (Utils.IsPointerOverUI("Menu", out name))
        // {
        //     if (name != uiElementName)
        //     {
        //         uiElementName = name;
        //         panel = GetPanelByName(uiElementName);
        //         panel.SetHighlight();
        //     }
        // }
        // else
        // {
        //     if (uiElementName != null)
        //     {
        //         panel = GetPanelByName(uiElementName);
        //         panel.SetDefault();
        //         uiElementName = null;
        //     }
        // }

        // if (Input.GetMouseButtonDown(0) && uiElementName != null)
        // {
        //     panel = GetPanelByName(uiElementName);
        //     panel.PlayChapter();

        //     /// close menu
        //     SideMenuCtrl.instance.Toggle();
        // }


        GameObject go;
        PanelMenuChapterCtrl panelChapter;
        PanelMenuChildrCtrl panelChild;

        if (Utils.IsPointerOverUI("Menu", out go))
        {
            if (go != uiGameobject)
            {
                if (uiGameobject != null)
                {
                    if (isPanelChild(uiGameobject, out panelChild))
                    {
                        panelChild.SetDefault();
                    }

                    // uiGameobject.GetComponent<PanelMenuChildrCtrl>().SetDefault();
                }

                uiGameobject = go;

                if (isPanelChild(uiGameobject, out panelChild))
                {
                    panelChild.SetHighlight();
                }

                // go.GetComponent<PanelMenuChildrCtrl>().SetHighlight();
            }
        }
        else
        {
            if (uiGameobject != null)
            {
                if (isPanelChild(uiGameobject, out panelChild))
                {
                    panelChild.SetDefault();
                }

                // uiGameobject.GetComponent<PanelMenuChildrCtrl>().SetDefault();
                uiGameobject = null;
            }
        }

        if (Input.GetMouseButtonDown(0) && uiGameobject != null)
        {
            menuPanelHasBeenClicked = true;

            uiGameobject.GetComponent<PanelMenuChildrCtrl>().PlayChild();

            /// close menu
            StartCoroutine(WaitToCloseMenu());
            // SideMenuCtrl.instance.Toggle();
        }

    }


    IEnumerator WaitToCloseMenu()
    {
        yield return new WaitForSeconds(0.2f);
        SideMenuCtrl.instance.OnToggle();
    }




    bool isPanelChild(GameObject go, out PanelMenuChildrCtrl _panel)
    {
        _panel = null;
        if (go.GetComponent<PanelMenuChildrCtrl>() != null)
        {
            _panel = go.GetComponent<PanelMenuChildrCtrl>();
            return true;
        }
        else
        {
            return false;
        }
    }


    public void SelectPanel(int number)
    {
        panel = GetPanelByNumber(number);
        panel.SetSelected();
        panelSelectedName = panel.prefabName;

        /// here we have to move the scroll rect to the selected panel
        // SnapTo(panel.gameObject.GetComponent<RectTransform>());
    }

    public void UnselectPanel()
    {
        if (panelSelectedName != null)
        {
            panel = GetPanelByName(panelSelectedName);
            panel.SetUnselected();
        }
    }

    public void SelectChildPanel(int number, int childNumber)
    {
        uiGameobjectSelected = allChilds[number][childNumber];
        uiGameobjectSelected.GetComponent<PanelMenuChildrCtrl>().SetSelected();

        SelectPanel(number);

        /// here we have to move the scroll rect to the selected panel
        if (menuPanelHasBeenClicked)
        {
            menuPanelHasBeenClicked = false;
        }
        else
        {
            // if (justStarted)
            // {
            //     justStarted = false;
            //     scrollViewport.normalizedPosition = new Vector2(0, 1);
            // }
            // else
            // {
            ScrollToCurrentElement();
            // }
        }

    }

    public void UnselectChildPanel()
    {
        if (uiGameobjectSelected != null)
        {
            uiGameobjectSelected.GetComponent<PanelMenuChildrCtrl>().SetUnselected();
        }

        UnselectPanel();
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

            /// create sub items
            List<GameObject> childs = new List<GameObject>();
            int ii = 0;
            foreach (GameObject child in chapter.childs)
            {
                GameObject newGo = Instantiate(childMenuPrefab, menuContainer);
                PanelMenuChildrCtrl _panelMenuChildCtrl = newGo.GetComponent<PanelMenuChildrCtrl>();
                _panelMenuChildCtrl.Setup(i, ii, child);

                childs.Add(newGo);
                ii++;
            }

            allChilds.Add(childs);
            i++;
        }
    }
}
