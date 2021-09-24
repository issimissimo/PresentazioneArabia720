using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Picture : MonoBehaviour
{
    public string pictureName;

    Material mat;

    Texture2D tex = null;
    byte[] fileData;


    public void Play(Action callback)
    {
        if (mat == null)
        {
            fileData = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, pictureName));
            Texture2D tex = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            mat = new Material(Shader.Find("Unlit/Texture"));
            mat.mainTexture = tex;
            Renderer rend = GetComponent<Renderer>();
            rend.material = mat;
        }

        StartCoroutine(_Play(callback));
    }

    IEnumerator _Play(Action callback)
    {
        yield return new WaitForSeconds(10);
        callback();
    }
}
