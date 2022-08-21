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
    [SerializeField] private Text coinText, diamondText;

    // Difficulty Panel
    private bool difficultyPanelHidden = true;

    // Buttons
    [SerializeField] private Button playButton, easyButton, mediumButton, hardButton, shopButton, closeShopButton;

    // Backgrounds
    [SerializeField] private GameObject[] backgrounds;

    // Panels + Animators
    [SerializeField] private GameObject difficultyPanel, shopPanel, buyBackgroundPanel, buyTrailPanel;
    [SerializeField] private Animator playButtonAnimator, difficultyPanelAnimator, shopPanelAnimator, buyBackgroundPanelAnimator, buyTrailPanelAnimator;
    [SerializeField] private const float animationTime = 1.1f; 

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
        // Setting UI Texts
        coinText.text = "" + GameData_Controller.SharedInstance.coins;
        diamondText.text = "" + GameData_Controller.SharedInstance.diamonds;

        backgrounds[GameData_Controller.SharedInstance.activeBackground].gameObject.SetActive(true);

        for(int i = 0; i < GameData_Controller.SharedInstance.backgroundsUnlocked.Length; i++)   // Setting the active and selected background
        {
            if (i == GameData_Controller.SharedInstance.activeBackground) backgrounds[i].gameObject.SetActive(true);
            else backgrounds[i].gameObject.SetActive(false);
        }
    }

    #region Buttons

    public void PlayButtonClicked()
    {
        StartCoroutine(PlayButtonClickedAnimation());
    }

    private IEnumerator PlayButtonClickedAnimation()
    {
        difficultyPanelAnimator.ResetTrigger("NotActive");
        playButtonAnimator.SetTrigger("NotActive");   // Out Animation
        difficultyPanel.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(animationTime);

        playButton.gameObject.SetActive(false);
        shopPanel.gameObject.SetActive(false);
        buyBackgroundPanel.gameObject.SetActive(false);
        buyTrailPanel.gameObject.SetActive(false);
    }

    public void CloseDifficultyPanelButtonClicked()
    {
        StartCoroutine(CloseDifficultyPanelButtonClickedAnimation());
    }

    private IEnumerator CloseDifficultyPanelButtonClickedAnimation()
    {
        playButtonAnimator.ResetTrigger("NotActive");   
        playButton.gameObject.SetActive(true);
        difficultyPanelAnimator.SetTrigger("NotActive");   // Out Animation

        yield return new WaitForSecondsRealtime(animationTime);

        difficultyPanel.gameObject.SetActive(false);
        shopPanel.gameObject.SetActive(false);
        buyBackgroundPanel.gameObject.SetActive(false);
        buyTrailPanel.gameObject.SetActive(false);
    }

    public void ShopButtonClicked()
    {
        StartCoroutine(ShopButtonClickedAnimation());
    }

    private IEnumerator ShopButtonClickedAnimation()
    {
        shopPanelAnimator.ResetTrigger("NotActive");
        difficultyPanelAnimator.SetTrigger("NotActive");
        shopPanel.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(animationTime);

        difficultyPanel.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        buyBackgroundPanel.gameObject.SetActive(false);
        buyTrailPanel.gameObject.SetActive(false);
    }

    public void CloseShopButtonClicked()
    {
        StartCoroutine(CloseShopButtonClickedAnimation());
    }

    private IEnumerator CloseShopButtonClickedAnimation()
    {
        shopPanelAnimator.SetTrigger("NotActive");
        playButtonAnimator.ResetTrigger("NotActive");
        playButton.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(animationTime);

        shopPanel.gameObject.SetActive(false);
        difficultyPanel.gameObject.SetActive(false);
        buyBackgroundPanel.gameObject.SetActive(false);
        buyTrailPanel.gameObject.SetActive(false);
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

    #region Screen Changer

    IEnumerator ScreenChangerTime()
    {
        yield return new WaitForSecondsRealtime(1.6f);
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadScene(1);
    }

    #endregion Screen Changer

    #endregion Methods
}
// EOF - End Of File