using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Controller : MonoBehaviour
{
    #region Variables

    // Shared Instance
    public static MainMenu_Controller SharedInstance;

    #region Pannels

    // UI
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private Text coinText, diamondText;

    // Difficulty Panel
    private bool difficultyPanelHidden = true;

    // Buttons
    [SerializeField] private Button playButton, easyButton, mediumButton, hardButton;

    #endregion Pannels

    #endregion Variables

    #region Methods

    private void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;
        UpdateUI();
        Shop_Controller.SharedInstancel.InitialiseShopUI();
    }

    public void UpdateUI()
    {
        coinText.text = "" + GameData_Controller.SharedInstance.coins;
        diamondText.text = "" + GameData_Controller.SharedInstance.diamonds;
    }

    #region Buttons

    public void PlayButtonClicked()
    {
        playButton.gameObject.SetActive(false);
        if (difficultyPanelHidden)
        {
            difficultyPanel.gameObject.SetActive(true);
            difficultyPanelHidden = false;
        }
    }

    public void CloseDifficultyPanelButtonClicked()
    {
        playButton.gameObject.SetActive(true);
        if (!difficultyPanelHidden)
        {
            difficultyPanel.gameObject.SetActive(false);
            difficultyPanelHidden = true;
        }
    }

    IEnumerator WaitHidingAnimation()
    {
        yield return new WaitForSecondsRealtime(1.2f);
        difficultyPanel.gameObject.SetActive(false);
        difficultyPanelHidden = true;
    }

    public void EasyButtonClicked()
    {
        GameData_Controller.SharedInstance.nextGameMode = 0;
        StartCoroutine(ScreenChangerTime());
    }

    public void MediumButtonClicked()
    {
        GameData_Controller.SharedInstance.nextGameMode = 1;
        StartCoroutine(ScreenChangerTime());
    }

    public void HardButtonClicked()
    {
        GameData_Controller.SharedInstance.nextGameMode = 2;
        StartCoroutine(ScreenChangerTime());
    }

    #endregion Buttons

    #region UI Animations

    IEnumerator ScreenChangerTime()
    {
        yield return new WaitForSecondsRealtime(1.6f);
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadScene(1);
    }

    #endregion UI Animations

    #endregion Methods
}
// EOF - End Of File