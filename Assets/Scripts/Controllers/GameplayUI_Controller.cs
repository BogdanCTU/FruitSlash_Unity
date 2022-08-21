using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUI_Controller : MonoBehaviour
{
    #region Variables

    // Shared Instance
    public static GameplayUI_Controller SharedInstance;

    #region UI_Elements

    // Gameplay UI
    [SerializeField] private Text scoreText, livesText, timerText;
    [SerializeField] private Animator gamePanelUIAnimator;

    // Pause Panel
    [SerializeField] private Button pauseButton, closeButton;
    [SerializeField] private GameObject gameUIPanel;
    [SerializeField] private Animator pausePanelAnimator;
    [SerializeField] private GameObject pausePanel;

    // Game Finished Panel
    [SerializeField] private Text bestScoreText, actualScoreText, livesLeftText;
    [SerializeField] private Animator gameFinishedPanelAnimator;
    [SerializeField] private GameObject gameFinishedPanel;
    // Extra Life
    [SerializeField] private Button extraLifeButton;
    private bool extraLifeUsed = false;

    // Starting Timer Panel
    [SerializeField] private Text startingTimerText;
    [SerializeField] private GameObject startingTimerPanel;
    public float startigTime = 3.9f;

    // Backgrounds Elements
    [SerializeField] private GameObject[] backgrounds;
    // Trails Elements
    [SerializeField] private GameObject[] trails;

    // Screen Changer
    [SerializeField] private Animator screenChangerPanel;

    // Animation Time
    [SerializeField] private const float animationTime = 1.1f;

    #endregion UI_Elements

    #endregion Variables

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;
        InitialiseUI();
        SetStartingTimer();
    }

    private void FixedUpdate()
    {
        if(startingTimerPanel.gameObject.activeInHierarchy) RunStartingTimer();
        UpdateUI();
        IsGameFinished();
    }

    #endregion Unity Methods

    #region UI

    public void InitialiseUI()
    {
        // Setting Panels
        gamePanelUIAnimator.SetTrigger("Active");
        gamePanelUIAnimator.ResetTrigger("NotActive");

        // UI Text
        scoreText.text = "0";   // Player Score

        // Setting Background
        for (int i = 0; i < GameData_Controller.SharedInstance.backgroundsUnlocked.Length; i++)   // Setting the active and selected background
        {
            if (i == GameData_Controller.SharedInstance.activeBackground) backgrounds[i].gameObject.SetActive(true);
            else backgrounds[i].gameObject.SetActive(false);
        }
    }

    public void UpdateUI()
    {
        // Player Score
        scoreText.text = "" + Gameplay_Controller.SharedInstance.actualScore;

        // Player Lives
        livesText.text = "" + Gameplay_Controller.SharedInstance.lives;

        // Time Left
        Gameplay_Controller.SharedInstance.timeLeft = Gameplay_Controller.SharedInstance.timeLeft - (Time.deltaTime / 2);
        if (Gameplay_Controller.SharedInstance.timeLeft >= 10) timerText.text = "00:" + (int)Gameplay_Controller.SharedInstance.timeLeft;
        else if (Gameplay_Controller.SharedInstance.timeLeft < 10) timerText.text = "00:0" + (int)Gameplay_Controller.SharedInstance.timeLeft;
    }

    #endregion UI

    #region Starting Timer

    public void SetStartingTimer()
    {
        startingTimerText.text = "" + (int)startigTime;
        startigTime = 3.9f;
        startingTimerPanel.gameObject.SetActive(true);
    }

    public void RunStartingTimer()
    {
        startigTime -= (Time.deltaTime / 2);
        startingTimerText.text = "" + (int)startigTime;
        if (startigTime == 0 || startigTime < 0)
        {
            startingTimerPanel.gameObject.SetActive(false);
            trails[GameData_Controller.SharedInstance.activeTrail].gameObject.SetActive(true);
        }
    }

    #endregion Starting Timer

    #region GameFinished

    public void IsGameFinished()
    {
        if (Gameplay_Controller.SharedInstance.timeLeft == 0 || Gameplay_Controller.SharedInstance.timeLeft < 0)
        {
            Gameplay_Controller.SharedInstance.PauseGame();
            GameFinishedPanel();

            // If time is over can not use extra life
            extraLifeUsed = true;
            extraLifeButton.gameObject.SetActive(false);
        }
        if (Gameplay_Controller.SharedInstance.lives == 0 || Gameplay_Controller.SharedInstance.lives < 0)
        {
            Gameplay_Controller.SharedInstance.PauseGame();
            GameFinishedPanel();

            // Showing or not ExtraLife Button
            if (extraLifeUsed == false && Gameplay_Controller.SharedInstance.actualScore > 1000)
            {
                extraLifeButton.gameObject.SetActive(true);
                extraLifeUsed = true;
            }
        }
    }

    public void GameFinishedPanel()
    {
        trails[GameData_Controller.SharedInstance.activeTrail].gameObject.SetActive(false);
        // Updating UI Text
        bestScoreText.text = "Best: " + GameData_Controller.SharedInstance.highScore;
        actualScoreText.text = "Actual: " + Gameplay_Controller.SharedInstance.actualScore;
        livesLeftText.text = "Left: " + Gameplay_Controller.SharedInstance.livesTemp;

        // Panels
        gameFinishedPanel.gameObject.SetActive(true);
        pausePanel.gameObject.SetActive(false);
        gameFinishedPanelAnimator.SetTrigger("Active");
        gameFinishedPanelAnimator.ResetTrigger("NotActive");
        gamePanelUIAnimator.ResetTrigger("Active");
        gamePanelUIAnimator.SetTrigger("NotActive");
    }

    #endregion GameFinished

    #region Button Functions

    public void PauseButtonClicked()
    {
        Gameplay_Controller.SharedInstance.PauseGame();

        pausePanelAnimator.ResetTrigger("NotActive");
        pausePanel.gameObject.SetActive(true);
        gameFinishedPanel.gameObject.SetActive(false);
        gamePanelUIAnimator.SetTrigger("NotActive");   // Out Animation
        gamePanelUIAnimator.ResetTrigger("Active");
    }

    public void ClosePauseButtonClicked()
    {
        StartCoroutine(CloseButtonClickedAnim());   // Waiting animation time
    }

    private IEnumerator CloseButtonClickedAnim()
    {
        pausePanelAnimator.SetTrigger("NotActive");   // Out Animation
        gamePanelUIAnimator.SetTrigger("Active");   // In Animation
        gamePanelUIAnimator.ResetTrigger("NotActive");

        yield return new WaitForSecondsRealtime(animationTime);

        pausePanel.gameObject.SetActive(false);
        Gameplay_Controller.SharedInstance.ResumeGame();
    }

    public void RestartButtonClicked()
    {
        StartCoroutine(RestartButtonClickedAnim());
    }

    private IEnumerator RestartButtonClickedAnim()
    {
        pausePanelAnimator.SetTrigger("NotActive");   // Out Animation
        gamePanelUIAnimator.SetTrigger("Active");   // In Animation
        gamePanelUIAnimator.ResetTrigger("NotActive");

        yield return new WaitForSecondsRealtime(animationTime);

        gameFinishedPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        SetStartingTimer();
        Gameplay_Controller.SharedInstance.ResumeGame();
        Gameplay_Controller.SharedInstance.InitialiseGameData();   // Resetting Gameplay Data
    }


    public void ReplayButtonClicked()
    {
        StartCoroutine(ReplayButtonClickedAnim());
    }

    private IEnumerator ReplayButtonClickedAnim()
    {
        Gameplay_Controller.SharedInstance.SaveGameData();   // Saving Game Data
        gameFinishedPanelAnimator.SetTrigger("NotActive");   // Out Animation
        gameFinishedPanelAnimator.ResetTrigger("Active");
        gamePanelUIAnimator.SetTrigger("Active");   // In Animation
        gamePanelUIAnimator.ResetTrigger("NotActive");

        yield return new WaitForSecondsRealtime(animationTime);

        gameFinishedPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        SetStartingTimer();
        Gameplay_Controller.SharedInstance.ResumeGame();
        Gameplay_Controller.SharedInstance.InitialiseGameData();   // Resetting Gameplay Data
    }

    #region Extra Life Button

    public void ExtraLifeButtonClicked()
    {
        StartCoroutine(ExtraLifeButtonClickedOutAnim());
    }

    private IEnumerator ExtraLifeButtonClickedOutAnim()
    {
        extraLifeUsed = true;   // Deactivating button

        // Subtracting currency
        Gameplay_Controller.SharedInstance.actualScore -= 1000;
        Gameplay_Controller.SharedInstance.ResumeGameExtra();

        gameFinishedPanelAnimator.SetTrigger("NotActive");   // Out Animation
        gameFinishedPanelAnimator.ResetTrigger("Active");
        gamePanelUIAnimator.ResetTrigger("NotActive");

        yield return new WaitForSecondsRealtime(1.1f);

        gameFinishedPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        extraLifeButton.gameObject.SetActive(false);
    }

    #endregion Extra Life Button

    #region Scene Changer

    public void MainMenuButtonClicked()
    {
        StartCoroutine(ChangeSceneMainMenu());   // Waiting animation time
    }

    IEnumerator ChangeSceneMainMenu()
    {
        Gameplay_Controller.SharedInstance.SaveGameData();
        //SceneManager.UnloadSceneAsync(1);
        screenChangerPanel.SetTrigger("Active");
        gamePanelUIAnimator.SetTrigger("NotActive");

        yield return new WaitForSecondsRealtime(1.1f);
        SceneManager.LoadScene(0);
    }

    #endregion Scene Changer

    #endregion Button Functions

    #endregion Methods
}
// EOF - End Of File