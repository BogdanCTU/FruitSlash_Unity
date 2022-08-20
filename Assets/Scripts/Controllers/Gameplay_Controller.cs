using UnityEngine;
using UnityEngine.UI;

public class Gameplay_Controller : MonoBehaviour
{
    #region Variables

    // Shared Instance
    public static Gameplay_Controller SharedInstance;

    #region Gameplay_Data

    public int actualScore, lives, gameMode = 1;   // gameMode {0 - easy, 1 - medium, 2 - hard}
    public float timeLeft;

    // Temp Data for Pause State
    public float timeTemp;
    public int scoreTemp, livesTemp;

    // Gameplay Spawners
    [SerializeField]
    private GameObject[] foodSpawners, skullSpawners;

    #endregion Gameplay_Data

    #endregion

    #region Methods

    private void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;
        InitialiseGameplay();
    }

    public void InitialiseGameplay()
    {
        gameMode = GameData_Controller.SharedInstance.nextGameMode;
        InitialiseGameMode();
        InitialiseGameData();
    }

    private void FixedUpdate()   // FixedUpdate is called once every fixed interval
                                 // (setted on project settings)
    {
        if (GameplayUI_Controller.SharedInstance.startigTime > 0) GameplayUI_Controller.SharedInstance.RunStartingTimer();
        GameplayUI_Controller.SharedInstance.UpdateUI();
        GameplayUI_Controller.SharedInstance.IsGameFinished();
    }

    #region Initialisators

    public void InitialiseGameData()
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

    private void InitialiseGameMode()
    {
        if (gameMode == 0)   // Easy
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

    #endregion Initialisators

    #region GameMode

    public void GetGameModeEnemy()   // Subtracting lives or time according to game difficulty
    {
        if (gameMode == 0)   // Easy
        {
            timeLeft -= 4.0f;
        }
        else if (gameMode == 1)   // Medium
        {
            timeLeft -= 2.5f;
            lives--;
        }
        else if (gameMode == 2)   // Hard
        {
            timeLeft -= 5.0f;
            lives--;
        }
    }

    public void GetGameModeFood(int points, int bonusTime)
    {
        if (gameMode == 0)   // Easy
        {
            actualScore += points;   // SetNewScore
            timeLeft += bonusTime;   // SetNewTime
        }
        else if (gameMode == 1)   // Medium
        {
            actualScore += (points * 2);   // SetNewScore
            timeLeft += (bonusTime / 2);   // SetNewTime
        }
        else if (gameMode == 2)   // Hard
        {
            actualScore += (points * 3);   // SetNewScore
            timeLeft += (bonusTime / 4);   // SetNewTime
        }
    }

    #endregion GameMode


    #region Pausing Game

    public void PauseGame()
    {
        // Deactivating Game Spawners
        foodSpawners[0].SetActive(false);
        foodSpawners[1].SetActive(false);
        foodSpawners[2].SetActive(false);
        foodSpawners[3].SetActive(false);
        skullSpawners[0].SetActive(false);
        skullSpawners[1].SetActive(false);

        SaveTempData();   // Saving Temp Game Data
    }

    public void ResumeGame()   // Reloading precedent Game Data
    {
        InitialiseGameMode();
        timeLeft = timeTemp;
        lives = livesTemp;
    }

    public void ResumeGameExtra()   // Reloading precedent Game Data
    {
        InitialiseGameMode();
        timeLeft = timeTemp;
        lives = 1;
    }

    public void SaveTempData()   // Need to save Temp Data in case of Pause State or Game Finished State
                                 // In order to resume previous Game State
    {
        timeTemp = timeLeft;
        timeLeft += 99999;
        livesTemp = lives;
        lives += 99999;
    }

    public void SaveGameData()
    {
        if (actualScore > GameData_Controller.SharedInstance.highScore) GameData_Controller.SharedInstance.highScore = actualScore;   // Setting High Score
        GameData_Controller.SharedInstance.coins += actualScore;
        GameData_Controller.SharedInstance.diamonds += livesTemp;
        GameData_Controller.SharedInstance.Save();
        Debug.Log("Saving GameData: {" + GameData_Controller.SharedInstance.highScore + "B$, " + GameData_Controller.SharedInstance.coins + " $, " + GameData_Controller.SharedInstance.diamonds + " <3");
    }

    #endregion Pausing Game

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