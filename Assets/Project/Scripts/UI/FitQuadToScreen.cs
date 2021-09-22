using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitQuadToScreen : MonoBehaviour
{
    void Start()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        transform.localScale = new Vector3(worldScreenWidth, worldScreenHeight, 1);
    }

    
}
