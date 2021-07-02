using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //スコア関連
    public Text scoreText;
    private int score;
    public int currentScore;
    public int clearScore = 1500;

    // タイマー関連
    public Text timerText;
    public float gameTime = 300.0f;
    int seconds;

    // UI関連
    public GameObject gamePauseUI;

    void Start()
    {
        Initialize();
    }

    
    void Update()
    {
        TimeManage();
    }

    // 初期状態にする
    private void Initialize()
    {
        score = 0;
    }

    // ゲーム時間の管理
    public void TimeManage()
    {
        gameTime -= Time.deltaTime;
        seconds = (int)gameTime;
        timerText.text = seconds.ToString();

        if (seconds == 0)
        {
            GameOver();
        }
    }

    // スコアの追加
    public void AddScore()
    {
        score += 100;
        currentScore += score;
        scoreText.text = "Score: " + currentScore.ToString();

        if (currentScore >= clearScore)
        {
            GameClear();
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Tetris");
    }

    public void GameClear()
    {
        SceneManager.LoadScene("Tetris");
    }

    public void GamePause()
    {
        GamePauseToggle();
    }

    public void GamePauseToggle()
    {
        gamePauseUI.SetActive(!gamePauseUI.activeSelf);

        if (gamePauseUI.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
