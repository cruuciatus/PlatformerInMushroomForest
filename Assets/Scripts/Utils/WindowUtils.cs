using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WindowUtils
{
    public static void CreateWindow(string resourcesPath)
    {
        var window = Resources.Load<GameObject>(resourcesPath);
        var canvas = GameObject.FindGameObjectWithTag("HudCanvas");// Object.FindObjectOfType<Canvas>();
        Object.Instantiate(window, canvas.transform);
    }

}
    

