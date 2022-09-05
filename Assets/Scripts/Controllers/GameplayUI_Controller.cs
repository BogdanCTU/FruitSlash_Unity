using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

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

    // Backgrounds & Trails Elements
    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private GameObject[] trails;

    // Screen Changer
    [SerializeField] private Animator screenChangerPanel;

    // Spam Click Blocker
    [SerializeField] private GameObject clickBlockerPanel;

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
        if (Gameplay_Controller.SharedInstance.timeLeft >= 10 && Gameplay_Controller.SharedInstance.timeLeft < 60) timerText.text = "00:" + (int)Gameplay_Controller.SharedInstance.timeLeft;
        else if (Gameplay_Controller.SharedInstance.timeLeft < 10) timerText.text = "00:0" + (int)Gameplay_Controller.SharedInstance.timeLeft;
        else if (Gameplay_Controller.SharedInstance.timeLeft >= 60) timerText.text = "01:00";
    }

    #endregion UI

    #region Starting Timer

    public void SetStartingTimer()
    {
        startingTimerText.text = "" + (int)startigTime;
        startigTime = 3.9f;
        startingTimerPanel.gameObject.SetActive(true);
        Gameplay_Controller.SharedInstance.gamePaused = true;
    }

    public void RunStartingTimer()
    {
        startigTime -= (Time.deltaTime / 2);
        startingTimerText.text = "" + (int)startigTime;
        if (startigTime == 0 || startigTime < 0)
        {
            startingTimerPanel.gameObject.SetActive(false);
            Gameplay_Controller.SharedInstance.gamePaused = false;
            trails[GameData_Controller.SharedInstance.activeTrail].gameObject.SetActive(true);
        }
    }

    #endregion Starting Timer

    #region GameFinished

    public void IsGameFinished()
    {
        if (Gameplay_Controller.SharedInstance.timeLeft == 0 || Gameplay_Controller.SharedInstance.timeLeft < 0)
        {
            StartCoroutine(GameFinishedPanelAnimation());

            // If time is over can not use extra life
            extraLifeUsed = true;
            extraLifeButton.gameObject.SetActive(false);
        }
        if (Gameplay_Controller.SharedInstance.lives == 0 || Gameplay_Controller.SharedInstance.lives < 0)
        {
            Gameplay_Controller.SharedInstance.lives = Gameplay_Controller.SharedInstance.lives < 0 ? 0 : Gameplay_Controller.SharedInstance.lives;
            StartCoroutine(GameFinishedPanelAnimation());

            // Showing or not ExtraLife Button
            if (extraLifeUsed == false && Gameplay_Controller.SharedInstance.actualScore > 250) extraLifeButton.gameObject.SetActive(true);
        }
    }

    private IEnumerator GameFinishedPanelAnimation()
    {
        Gameplay_Controller.SharedInstance.gamePaused = true;
        clickBlockerPanel.gameObject.SetActive(true);

        // Updating UI Text
        bestScoreText.text = Gameplay_Controller.SharedInstance.actualScore < GameData_Controller.SharedInstance.highScore ? "Best: " + GameData_Controller.SharedInstance.highScore : "Best: " + Gameplay_Controller.SharedInstance.actualScore;
        actualScoreText.text = "Actual: " + Gameplay_Controller.SharedInstance.actualScore;
        livesLeftText.text = "Left: " + Gameplay_Controller.SharedInstance.lives;
        Gameplay_Controller.SharedInstance.PauseGame();

        // Panels & Animations
        gameFinishedPanelAnimator.ResetTrigger("NotActive");
        gameFinishedPanel.gameObject.SetActive(true);   // Activate + Animation
        gamePanelUIAnimator.SetTrigger("NotActive");
        gamePanelUIAnimator.ResetTrigger("Active");
        pausePanel.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(animationTime);

        clickBlockerPanel.gameObject.SetActive(false);
    }

    #endregion GameFinished

    #region Button Functions

    public void PauseButtonClicked()
    {
        Sound_Controller.SharedInstance.PlayButtonSound();
        StartCoroutine(PauseButtonClickedAnimation());
    }

    private IEnumerator PauseButtonClickedAnimation()
    {
        Gameplay_Controller.SharedInstance.gamePaused = true;
        clickBlockerPanel.gameObject.SetActive(true);

        // Panels & Animations
        pausePanelAnimator.ResetTrigger("NotActive");
        pausePanel.gameObject.SetActive(true);   // In Animation
        gameFinishedPanel.gameObject.SetActive(false);
        gamePanelUIAnimator.SetTrigger("NotActive");   // Out Animation
        gamePanelUIAnimator.ResetTrigger("Active");

        yield return new WaitForSecondsRealtime(animationTime);   // Waiting Animation time

        Gameplay_Controller.SharedInstance.PauseGame();
        clickBlockerPanel.gameObject.SetActive(false);
    }

    public void ClosePauseButtonClicked()
    {
        Sound_Controller.SharedInstance.PlayDontBuyButtonSound();
        StartCoroutine(CloseButtonClickedAnimation());
    }

    private IEnumerator CloseButtonClickedAnimation()
    {
        Gameplay_Controller.SharedInstance.ResumeGame();
        clickBlockerPanel.gameObject.SetActive(true);

        // Panels & Animations
        pausePanelAnimator.SetTrigger("NotActive");   // Out Animation
        gamePanelUIAnimator.SetTrigger("Active");   // In Animation
        gamePanelUIAnimator.ResetTrigger("NotActive");

        yield return new WaitForSecondsRealtime(animationTime);   // Waiting Animation time

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        pausePanel.gameObject.SetActive(false);

        clickBlockerPanel.gameObject.SetActive(false);
    }

    public void RestartButtonClicked()
    {
        Sound_Controller.SharedInstance.PlayBuyButtonSound();
        StartCoroutine(RestartButtonClickedAnimation());
    }

    private IEnumerator RestartButtonClickedAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);

        pausePanelAnimator.SetTrigger("NotActive");   // Out Animation

        yield return new WaitForSecondsRealtime(animationTime);   // Waiting Animation time

        gamePanelUIAnimator.SetTrigger("Active");   // In Animation
        gamePanelUIAnimator.ResetTrigger("NotActive");
        Gameplay_Controller.SharedInstance.ResumeGame();
        SetStartingTimer();   // Restarting Gameplay
        Gameplay_Controller.SharedInstance.InitialiseGameData();   // Resetting Gameplay Data
        extraLifeUsed = false;

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        gameFinishedPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);

        clickBlockerPanel.gameObject.SetActive(false);
    }

    public void ReplayButtonClicked()
    {
        Sound_Controller.SharedInstance.PlayBuyButtonSound();
        StartCoroutine(ReplayButtonClickedAnimation());
    }

    private IEnumerator ReplayButtonClickedAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);

        Gameplay_Controller.SharedInstance.SaveGameData();   // Saving Game Data

        gameFinishedPanelAnimator.SetTrigger("NotActive");   // Out Animation
        gameFinishedPanelAnimator.ResetTrigger("Active");

        yield return new WaitForSecondsRealtime(animationTime);   // Waiting Animation time

        gamePanelUIAnimator.SetTrigger("Active");   // In Animation
        gamePanelUIAnimator.ResetTrigger("NotActive");

        Gameplay_Controller.SharedInstance.ResumeGame();
        SetStartingTimer();   // Restarting Gameplay
        Gameplay_Controller.SharedInstance.InitialiseGameData();   // Resetting Gameplay Data
        extraLifeUsed = false;

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        gameFinishedPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);

        clickBlockerPanel.gameObject.SetActive(false);
    }

    #region Extra Life Button

    public void ExtraLifeButtonClicked()
    {
        Sound_Controller.SharedInstance.PlayBuyButtonSound();
        StartCoroutine(ExtraLifeButtonClickedOutAnimation());
    }

    private IEnumerator ExtraLifeButtonClickedOutAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        extraLifeUsed = true;   // Deactivating button

        // Subtracting currency
        Gameplay_Controller.SharedInstance.actualScore -= 250;
        Gameplay_Controller.SharedInstance.ResumeGameExtra();

        gameFinishedPanelAnimator.SetTrigger("NotActive");   // Out Animation
        gameFinishedPanelAnimator.ResetTrigger("Active");
        gamePanelUIAnimator.ResetTrigger("NotActive");
        gamePanelUIAnimator.SetTrigger("Active");

        yield return new WaitForSecondsRealtime(animationTime);   // Waiting Animation time

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        gameFinishedPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        extraLifeButton.gameObject.SetActive(false);
        clickBlockerPanel.gameObject.SetActive(false);
    }

    #endregion Extra Life Button

    #region Scene Changer

    public void MainMenuButtonClicked()
    {
        Sound_Controller.SharedInstance.PlayDontBuyButtonSound();
        StartCoroutine(MainMenuButtonClickedAnimation());
    }

    IEnumerator MainMenuButtonClickedAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        Gameplay_Controller.SharedInstance.SaveGameData();
        //SceneManager.UnloadSceneAsync(1);
        screenChangerPanel.SetTrigger("Active");
        gamePanelUIAnimator.SetTrigger("NotActive");

        yield return new WaitForSecondsRealtime(animationTime);   // Waiting Animation time
        SceneManager.LoadScene(0);
    }

    #endregion Scene Changer

    #endregion Button Functions

    #endregion Methods
}
// EOF - End Of File