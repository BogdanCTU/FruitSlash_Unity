using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUI_Controller : MonoBehaviour
{
    #region Variables

    // Shared Instance
    public static GameplayUI_Controller SharedInstance;

    #region UI_Elements

    // UI Text
    [SerializeField] private Text scoreText, livesText, timerText;

    // Pause
    [SerializeField] private Button pauseButton, closeButton;
    [SerializeField] private GameObject gameFinishedPanel, gameUIPanel, pausePanel;

    // Extra Life
    [SerializeField] private Button extraLifeButton;
    private bool extraLifeUsed = false;

    // Game Finished
    [SerializeField] private Text bestScoreText, actualScoreText, livesLeftText;

    // Starting Timer
    [SerializeField] private Text startingTimerText;
    [SerializeField] private GameObject startingTimerPanel;
    public float startigTime = 5.0f;
    [SerializeField] private GameObject SwipeTrail;

    #endregion UI_Elements

    #endregion

    #region Methods

    private void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;
        SetStartingTimer();
        InitialiseUI();
    }

    private void FixedUpdate()
    {
        if(startingTimerPanel.gameObject.activeInHierarchy) RunStartingTimer();
        UpdateUI();
        IsGameFinished();
    }

    #region UI

    public void InitialiseUI()
    {
        // Player Score
        scoreText.text = "0";
    }

    public void UpdateUI()
    {
        // Player Score
        scoreText.text = "" + Gameplay_Controller.SharedInstance.actualScore;

        // Player Lives
        livesText.text = "" + Gameplay_Controller.SharedInstance.lives;

        // Time Left
        Gameplay_Controller.SharedInstance.timeLeft = Gameplay_Controller.SharedInstance.timeLeft - Time.deltaTime;
        if (Gameplay_Controller.SharedInstance.timeLeft >= 10) timerText.text = "00:" + (int)Gameplay_Controller.SharedInstance.timeLeft;
        else if (Gameplay_Controller.SharedInstance.timeLeft < 10) timerText.text = "00:0" + (int)Gameplay_Controller.SharedInstance.timeLeft;
    }

    #endregion UI

    #region Starting Timer

    public void SetStartingTimer()
    {
        startingTimerText.text = "" + (int)startigTime;
        startigTime = 5.0f;
        startingTimerPanel.gameObject.SetActive(true);
    }

    public void RunStartingTimer()
    {
        startigTime -= Time.deltaTime;
        startingTimerText.text = "" + (int)startigTime;
        if (startigTime == 0 || startigTime < 0)
        {
            startingTimerPanel.gameObject.SetActive(false);
            SwipeTrail.gameObject.SetActive(true);
        }
    }


    #endregion Starting Timer

    #region GameFinished

    public void IsGameFinished()
    {
        if (Gameplay_Controller.SharedInstance.timeLeft == 0 || Gameplay_Controller.SharedInstance.timeLeft < 0)
        {
            // Changing Pannel and Stopping Game Pausing
            Gameplay_Controller.SharedInstance.PauseGame();
            gameUIPanel.SetActive(false);
            gameFinishedPanel.SetActive(true); GameFinishedPanel();
            extraLifeUsed = true;
            extraLifeButton.gameObject.SetActive(false);
        }
        if (Gameplay_Controller.SharedInstance.lives == 0 || Gameplay_Controller.SharedInstance.lives < 0)
        {
            // Changing Pannel and Stopping Game Pausing
            Gameplay_Controller.SharedInstance.PauseGame();
            gameUIPanel.SetActive(false);
            gameFinishedPanel.SetActive(true); GameFinishedPanel();

            // Showing or not ExtraLife Button
            if (extraLifeUsed == false && Gameplay_Controller.SharedInstance.actualScore > 1000)
            {
                extraLifeButton.gameObject.SetActive(true);
                extraLifeUsed = true;
            }
        }
    }

    private void GameFinishedPanel()
    {
        SwipeTrail.gameObject.SetActive(false);
        // Updating UI Text
        bestScoreText.text = "Best: " + GameData_Controller.SharedInstance.highScore;
        actualScoreText.text = "Actual: " + Gameplay_Controller.SharedInstance.actualScore;
        livesLeftText.text = "Left: " + Gameplay_Controller.SharedInstance.livesTemp;
    }

    #endregion GameFinished

    #region Button Functions

    public void PauseButtonClicked()
    {
        gameUIPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(true);
        Gameplay_Controller.SharedInstance.PauseGame();
    }

    public void CloseButtonClicked()
    {
        Gameplay_Controller.SharedInstance.ResumeGame();
        gameUIPanel.gameObject.SetActive(true);
        pausePanel.gameObject.SetActive(false);
    }

    public void RestartButtonClicked()
    {
        // Changing UI Panels
        gameUIPanel.gameObject.SetActive(true);
        pausePanel.gameObject.SetActive(false);
        SetStartingTimer();
        Gameplay_Controller.SharedInstance.ResumeGame();

        // Resetting Gameplay Data
        Gameplay_Controller.SharedInstance.InitialiseGameData();
    }

    public void ReplayButtonClicked()
    {
        // Saving Game Data
        Gameplay_Controller.SharedInstance.SaveGameData();

        // Changing UI Panels
        gameUIPanel.gameObject.SetActive(true);
        gameFinishedPanel.gameObject.SetActive(false);
        SetStartingTimer();
        Gameplay_Controller.SharedInstance.ResumeGame();

        // Resetting Gameplay Data
        Gameplay_Controller.SharedInstance.InitialiseGameData();
    }

    public void ExtraLifeButtonClicked()
    {
        // Deactivating button
        extraLifeButton.gameObject.SetActive(false);   
        extraLifeUsed = true;

        // Subtracting currency
        Gameplay_Controller.SharedInstance.actualScore -= 1000;   
        Gameplay_Controller.SharedInstance.ResumeGameExtra();

        // Changing UI Panels
        gameUIPanel.gameObject.SetActive(true);
        gameFinishedPanel.gameObject.SetActive(false);
    }

    public void MainMenuButtonClicked()
    {
        Gameplay_Controller.SharedInstance.SaveGameData();   // Saving Game Data
        SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene(0);   // Changing Scene to MainMenu
    }

    #endregion Button Functions

    #endregion
}
   // EOF - End Of File