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
    [SerializeField] private Text scoreText, livestext, timerText;

    // Pause
    [SerializeField] private Button pauseButton, closeButton;
    [SerializeField] private GameObject gameFinishedPanel, gameUIPanel, pausePanel;

    // Extra Life
    [SerializeField] private Button extraLifeButton;
    private bool extraLifeUsed = false;

    // Game Finished
    [SerializeField] private Text bestScoreText, actualScoreText, livesLeftText;

    #endregion UI_Elements

    #endregion

    #region Methods

    private void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;
    }

    private void FixedUpdate()
    {
        InitialiseUI();
        UpdateUI();
        IsGameFinished();
    }

    #region UI

    public void InitialiseUI()
    {
        // Player Score
        scoreText.text = "0";

        // Player Lives
        livestext.text = "" + Gameplay_Controller.SharedInstance.lives;

        // Time Left
        timerText.text = "" + (int)Gameplay_Controller.SharedInstance.timeLeft;
    }

    public void UpdateUI()
    {
        // Player Score
        scoreText.text = "" + Gameplay_Controller.SharedInstance.actualScore;

        // Player Lives
        livestext.text = "" + Gameplay_Controller.SharedInstance.lives;

        // Time Left
        Gameplay_Controller.SharedInstance.timeLeft = Gameplay_Controller.SharedInstance.timeLeft - Time.deltaTime;
        if (Gameplay_Controller.SharedInstance.timeLeft >= 10) timerText.text = "00:" + (int)Gameplay_Controller.SharedInstance.timeLeft;
        else if (Gameplay_Controller.SharedInstance.timeLeft < 10) timerText.text = "00:0" + (int)Gameplay_Controller.SharedInstance.timeLeft;
    }

    #endregion UI

    #region GameFinished

    public void IsGameFinished()
    {
        if (Gameplay_Controller.SharedInstance.timeLeft == 0 || Gameplay_Controller.SharedInstance.timeLeft < 0)
        {
            // Changing Pannel and Stopping Game Pausing
            Gameplay_Controller.SharedInstance.PauseGame();
            gameUIPanel.SetActive(false);
            gameFinishedPanel.SetActive(true);
            extraLifeUsed = true;
            extraLifeButton.gameObject.SetActive(false);
        }
        if (Gameplay_Controller.SharedInstance.lives == 0 || Gameplay_Controller.SharedInstance.lives < 0)
        {
            // Changing Pannel and Stopping Game Pausing
            Gameplay_Controller.SharedInstance.PauseGame();
            gameUIPanel.SetActive(false);
            gameFinishedPanel.SetActive(true);

            // Showing or not ExtraLife Button
            if (extraLifeUsed == false && Gameplay_Controller.SharedInstance.actualScore > 1000)
            {
                extraLifeButton.gameObject.SetActive(true);
                extraLifeUsed = true;
            }
        }
    }

    #endregion GameFinished

    #region GamePaused

    #endregion GamePaused

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
        Gameplay_Controller.SharedInstance.ResumeGame();

        // Resetting Gameplay Data
        Gameplay_Controller.SharedInstance.InitialiseGameData();
    }

    public void ReplayButtonClicked()
    {
        // Changing UI Panels
        gameUIPanel.gameObject.SetActive(true);
        pausePanel.gameObject.SetActive(false);
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
        SceneManager.LoadScene(0);
    }

    #endregion Button Functions

    #endregion
}
   // EOF - End Of File