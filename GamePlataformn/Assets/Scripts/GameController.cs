using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public int Score;
    public Text scoreText;

    public GameObject gameOverPanel;
    public GameObject gameWinPanel;

    public static GameController instance;

    private void Awake()
    {

        instance = this;

        Time.timeScale = 1;
        //DontDestroyOnLoad(this);

        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        if (PlayerPrefs.GetInt("score") > 0)
        {
            Score = PlayerPrefs.GetInt("score");
            scoreText.text = "X " + Score.ToString();
        }

        //PlayerPrefs.DeleteAll();
    }

    public void GetCoin()
    {
        Score++;
        scoreText.text = "X " + Score.ToString();

        PlayerPrefs.SetInt("score", Score);
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void ShowGameWin()
    {
        Time.timeScale = 0;
        gameWinPanel.SetActive(true);
    }

    public void exitGame()
    {
        Debug.Log("Saiu");
        Application.Quit();
    }

    public void RestartGame()
    {
        PlayerPrefs.DeleteAll();
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        SceneManager.LoadScene(1);
    }

}
