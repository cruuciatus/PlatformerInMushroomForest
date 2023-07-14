using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOver;

    public void OverGame()
    {

        gameOver.SetActive(true);
    }

    public void OnStartGame()
    {
        StateLoadGame.IsBegin = true;
        SceneManager.LoadScene("Level1-Mage");

    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
