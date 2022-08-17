using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay_Controller : MonoBehaviour
{
    #region Variables

    // Shared Instance
    public static Gameplay_Controller SharedInstance;

    #region Gameplay_Data

    private int actualScore, lives;
    public int gameMode = 1;   // gameMode {0 - easy, 1 - medium, 2 - hard}
    private float timeLeft;

    // Gameplay Spawners
    [SerializeField]
    private GameObject[] foodSpawners, skullSpawners;

    #endregion Gameplay_Data

    #region UI_Elements

    [SerializeField]
    private Text scoreText, livestext, timerText;

    #endregion UI_Elements

    #endregion

    #region Methods

    private void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;
        InitialiseGameplay();
    }

    public void InitialiseGameplay()
    {
        InitialiseGameMode();
        InitialiseGameData();
        InitialiseUI();
    }

    private void FixedUpdate()   // FixedUpdate is called once every fixed interval
                                 // (setted on project settings)
    {
        UpdateUI();
        IsGameFinished();
    }

    private void InitialiseGameMode()
    {
        if(gameMode == 0)   // Easy
        {
            foodSpawners[0].SetActive(true);
            foodSpawners[1].SetActive(true);
            foodSpawners[2].SetActive(true);
            foodSpawners[3].SetActive(false);
            skullSpawners[0].SetActive(true);
            skullSpawners[1].SetActive(false);
        }
        else if (gameMode == 1)   // Medium
        {
            foodSpawners[0].SetActive(true);
            foodSpawners[1].SetActive(true);
            foodSpawners[2].SetActive(true);
            foodSpawners[3].SetActive(false);
            skullSpawners[0].SetActive(true);
            skullSpawners[1].SetActive(false);
        }
        else if (gameMode == 2)   // Hard
        {
            foodSpawners[0].SetActive(true);
            foodSpawners[1].SetActive(true);
            foodSpawners[2].SetActive(true);
            foodSpawners[3].SetActive(true);
            skullSpawners[0].SetActive(true);
            skullSpawners[1].SetActive(true);
        }
    }

    #region Initialisators
    private void InitialiseGameData()
    {
        if(gameMode == 0 || gameMode == 1 || gameMode == 2)
        {
            actualScore = 0;
            lives = 3;
            timeLeft = 60.0f;
        }
        else if(gameMode == 3)
        {
            actualScore = 0;
            lives = 1;
            timeLeft = 30.0f;
        }
    }

    private void InitialiseUI()
    {
        // Player Score
        scoreText.text = "Score: " + actualScore;

        // Player Lives
        livestext.text = "Lives: " + lives;

        // Time Left
        timerText.text = "00:" + (int)timeLeft;
    }
    #endregion Initialisators

    private void UpdateUI()
    {
        // Player Score
        scoreText.text = "Score: " + actualScore;

        // Player Lives
        livestext.text = "Lives: " + lives;

        // Time Left
        timeLeft = timeLeft - Time.deltaTime;
        if (timeLeft >= 10) timerText.text = "00:" + (int)timeLeft;
        else if (timeLeft < 10) timerText.text = "00:0" + (int)timeLeft;
    }

    private void IsGameFinished()
    {
        
    }

    #region Getters/Setters

    // Score
    public void SetActualScore(int score) { this.actualScore = score; }
    public void AddScore(int score) { this.actualScore += score; }
    public int GetActualScore() { return this.actualScore; }

    // Time
    public void SetTimeLeft(float time) { this.timeLeft = time; }
    public void AddTime(float time) { this.timeLeft += time; }
    public void SubtractTime(float time) { this.timeLeft -= time; }
    public float GetTimeLeft() { return this.timeLeft; }

    // Lives
    public void SetLivesLeft(int newLives) { this.lives = newLives; }
    public void AddLife() { this.lives += 1; }
    public void SubtractLife() { this.lives -= 1; }
    public float GetLivesLeft() { return this.lives; }

    // Game Mode
    public void SetGameMode(int newGameMode) { this.gameMode = newGameMode; }
    public int GetGameMode() { return this.gameMode; }

    #endregion Getters/Setters

    #endregion
}
   // EOF - End Of File