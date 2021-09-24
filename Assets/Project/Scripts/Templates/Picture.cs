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

    Coroutine waitCoroutine;
    Action callback;

    private void OnEnable() {
        GameManager.OnPlayAutoToggleEvent += OnPlayAutoToggle;
    }

    private void OnDisable() {
        GameManager.OnPlayAutoToggleEvent -= OnPlayAutoToggle;
    }


    public void Play(Action _callback)
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

        callback = _callback;

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(10);
        waitCoroutine = null;
        callback();
    }


    void OnPlayAutoToggle(bool value){
        if (value){
            if (waitCoroutine == null) waitCoroutine = StartCoroutine(Wait());
        }
    }
}
