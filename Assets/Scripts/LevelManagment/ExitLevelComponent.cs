using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevelComponent : MonoBehaviour
{
    [SerializeField] private string _sceneName;

   public void Exit()
    {
        var session = FindObjectOfType<GameSession>();
        session.Save();
        session.NextLevel();
        SceneManager.LoadScene(_sceneName);
    }
}
