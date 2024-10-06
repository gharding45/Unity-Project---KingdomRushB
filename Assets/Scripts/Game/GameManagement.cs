using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{

    public GameObject gameOverUI;
    public GameObject gameWonUI;

    public static bool gameOver;


    void Start()
    {
        gameOver = false;
    }

    void Update()
    {
        if (gameOver)
        {
            return;
        }
    
        if (Stats.playerHealth <= 0)
        {
        	EndGame();
        }
    }

    public void EndGame()
    {
        gameOver = true;
        gameOverUI.SetActive(true);
    }

    public void WinGame()
    {
        UserStats.setGameWon(true);
        gameOver = true;
        gameWonUI.SetActive(true);
    }
}
