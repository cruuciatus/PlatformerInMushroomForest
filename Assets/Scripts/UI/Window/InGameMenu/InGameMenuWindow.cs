using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class InGameMenuWindow : AnimatedWindow
{
    private float _defaultTimeScale;
    protected override void Start()
    {
        base.Start();
        _defaultTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }
    public void InShowSettings()
    {
        WindowUtils.CreateWindow("UI/SettingsWindow");
    }
    public void OnExit()
    {
        SceneManager.LoadScene("MainMenu");
        var session = FindObjectOfType<GameSession>();
        Destroy(session.gameObject);

       
    }


    private void OnDestroy()
    {
        Time.timeScale = _defaultTimeScale;
    }
}
