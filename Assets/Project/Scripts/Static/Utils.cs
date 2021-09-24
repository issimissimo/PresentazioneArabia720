using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    public static bool IsPointerOverUI(string name)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (RaycastResult raycastResult in raycastResults)
        {
            if (raycastResult.gameObject.name == name)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsPointerOverUI(string tag, out string name)
    {
        name = null;
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (RaycastResult raycastResult in raycastResults)
        {
            //  Debug.Log(raysastResult.gameObject.name);
            if (raycastResult.gameObject.CompareTag(tag))
            {
                name = raycastResult.gameObject.name;
                return true;
            }
        }
        return false;
    }
}
