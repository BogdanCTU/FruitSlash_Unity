using UnityEngine;

public class Gameplay_Controller : MonoBehaviour
{
    #region Variables

    // Shared Instance
    public static Gameplay_Controller SharedInstance;

    #region Gameplay_Data

    public int actualScore, lives, gameMode = 1;   // gameMode {0 - easy, 1 - medium, 2 - hard}
    public float timeLeft;

    // Temp Data for Pause State
    public bool gamePaused = true;
    public float timeTemp;
    public int scoreTemp, livesTemp;

    // Gameplay Spawners
    [SerializeField]
    private GameObject[] foodSpawners, skullSpawners;

    // Combo System
    public int comboCount = 0;
    public float comboTime = 0f;

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
        ComboTime();
    }

    #region Initialisators

    public void InitialiseGameData()
    {
        switch (gameMode)
        {
            case 0:   // Easy
                {
                    actualScore = 0;
                    lives = 5;
                    timeLeft = 64.0f;
                    break;
                }
            case 1:   // Medium
                {
                    actualScore = 0;
                    lives = 3;
                    timeLeft = 64.0f;
                    break;
                }
            case 2:   // Hard
                {
                    actualScore = 0;
                    lives = 3;
                    timeLeft = 50.0f;
                    break;
                }
            default:   // Explicit -> medium
                {
                    actualScore = 0;
                    lives = 3;
                    timeLeft = 64.0f;
                    break;
                }
        }
    }

    private void InitialiseGameMode()
    {
        switch (gameMode)
        {
            case 0:   // Easy
                {
                    foodSpawners[0].SetActive(true);
                    foodSpawners[1].SetActive(true);
                    foodSpawners[2].SetActive(true);
                    foodSpawners[3].SetActive(false);
                    skullSpawners[0].SetActive(true);
                    skullSpawners[1].SetActive(false);
                    break;
                }
            case 1:   // Medium
                {
                    foodSpawners[0].SetActive(true);
                    foodSpawners[1].SetActive(true);
                    foodSpawners[2].SetActive(true);
                    foodSpawners[3].SetActive(false);
                    skullSpawners[0].SetActive(true);
                    skullSpawners[1].SetActive(false);
                    break;
                }
            case 2:   // Hard
                {
                    foodSpawners[0].SetActive(true);
                    foodSpawners[1].SetActive(true);
                    foodSpawners[2].SetActive(true);
                    foodSpawners[3].SetActive(true);
                    skullSpawners[0].SetActive(true);
                    skullSpawners[1].SetActive(true);
                    break;
                }
            default:   // Explicit -> Medium
                {
                    foodSpawners[0].SetActive(true);
                    foodSpawners[1].SetActive(true);
                    foodSpawners[2].SetActive(true);
                    foodSpawners[3].SetActive(false);
                    skullSpawners[0].SetActive(true);
                    skullSpawners[1].SetActive(false);
                    break;
                }
        }
    }

    #endregion Initialisators

    #region GameMode

    public void GetGameModeEnemy()   // Subtracting lives or time according to game difficulty
    {
        switch (gameMode)
        {
            case 0:   // Easy
                {
                    timeLeft -= 4.0f;
                    lives--;
                    break;
                }
            case 1:   // Medium
                {
                    timeLeft -= 2.5f;
                    lives--;
                    break;
                }
            case 2:   // Hard
                {
                    timeLeft -= 5.0f;
                    lives--;
                    break;
                }
            default:   // Explicit -> Medium
                {
                    timeLeft -= 2.5f;
                    lives--;
                    break;
                }
        }
    }

    public void GetGameModeFood(int points, int bonusTime)
    {
        switch (gameMode)
        {
            case 0:   // Easy
                {
                    actualScore += points;   // Set new score
                    if (timeLeft < 59.0) timeLeft += bonusTime;   // Set new time
                    break;
                }
            case 1:   // Medium
                {
                    actualScore += (points * 2);   // Set new score
                    if (timeLeft < 59.0) timeLeft += (bonusTime / 2);   // Set new time
                    break;
                }
            case 2:   // Hard
                {
                    actualScore += (points * 3);   // Set new score
                    if (timeLeft < 59.0) timeLeft += (bonusTime / 4);   // Set new time
                    break;
                }
            default:   // Explicit -> Medium
                {
                    actualScore += (points * 2);   // Set new score
                    if (timeLeft < 59.0) timeLeft += (bonusTime / 2);   // Set new time
                    break;
                }
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

        gamePaused = true;
        SaveTempData();   // Saving Temp Game Data
    }

    public void ResumeGame()   // Reloading precedent Game Data
    {
        gamePaused = false;
        InitialiseGameMode();
        timeLeft = timeTemp;
        lives = livesTemp;
    }

    public void ResumeGameExtra()   // Reloading precedent Game Data
    {
        gamePaused = false;
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
        GameData_Controller.SharedInstance.Save();
    }

    #endregion Pausing Game

    #region Getters/Setters
    
    public int GetGameMode()
    {
        return gameMode;
    }

    #endregion Getters/Setters

    private void ComboTime()
    {
        comboTime = comboTime > 0 ? comboTime - (Time.deltaTime / 2) : (comboTime < 0 ? 0 : comboTime);
        if (comboTime == 0) comboCount = 0;
    }

    #endregion Methods
}
// EOF - End Of File