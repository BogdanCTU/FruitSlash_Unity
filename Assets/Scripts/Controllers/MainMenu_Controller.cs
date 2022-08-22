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
    [SerializeField] private Animator shopButtonAnimator;

    // Backgrounds
    [SerializeField] private GameObject[] backgrounds;

    // Panels + Animators
    [SerializeField] private GameObject difficultyPanel, shopPanel, buyBackgroundPanel, buyTrailPanel;
    [SerializeField] private Animator playButtonAnimator, difficultyPanelAnimator, shopPanelAnimator, buyBackgroundPanelAnimator, buyTrailPanelAnimator;
    [SerializeField] private const float animationTime = 1.1f;

    // Backgrounds
    [SerializeField] private GameObject clickBlockerPanel;

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
        Sound_Controller.SharedInstance.PlayButtonSound();
        StartCoroutine(PlayButtonClickedAnimation());
    }

    private IEnumerator PlayButtonClickedAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        // Animations
        difficultyPanelAnimator.ResetTrigger("NotActive");
        playButtonAnimator.SetTrigger("NotActive");
        difficultyPanel.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(animationTime);

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        playButton.gameObject.SetActive(false);
        shopPanel.gameObject.SetActive(false);
        buyBackgroundPanel.gameObject.SetActive(false);
        buyTrailPanel.gameObject.SetActive(false);
        clickBlockerPanel.gameObject.SetActive(false);
    }

    public void CloseDifficultyPanelButtonClicked()
    {
        Sound_Controller.SharedInstance.PlayDontBuyButtonSound();
        StartCoroutine(CloseDifficultyPanelButtonClickedAnimation());
    }

    private IEnumerator CloseDifficultyPanelButtonClickedAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        // Animations
        playButtonAnimator.ResetTrigger("NotActive");   
        playButton.gameObject.SetActive(true);
        difficultyPanelAnimator.SetTrigger("NotActive");

        yield return new WaitForSecondsRealtime(animationTime);

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        difficultyPanel.gameObject.SetActive(false);
        shopPanel.gameObject.SetActive(false);
        buyBackgroundPanel.gameObject.SetActive(false);
        buyTrailPanel.gameObject.SetActive(false);
        clickBlockerPanel.gameObject.SetActive(false);
    }

    public void ShopButtonClicked()
    {

        Sound_Controller.SharedInstance.PlayButtonSound();
        StartCoroutine(ShopButtonClickedAnimation());
    }

    private IEnumerator ShopButtonClickedAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        // Animations
        shopButtonAnimator.SetTrigger("NotActive");
        shopPanelAnimator.ResetTrigger("NotActive");
        difficultyPanelAnimator.SetTrigger("NotActive");
        shopPanel.gameObject.SetActive(true);
        
        yield return new WaitForSecondsRealtime(animationTime);

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        shopButton.gameObject.SetActive(false);
        difficultyPanel.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        buyBackgroundPanel.gameObject.SetActive(false);
        buyTrailPanel.gameObject.SetActive(false);
        clickBlockerPanel.gameObject.SetActive(false);
    }

    public void CloseShopButtonClicked()
    {
        Sound_Controller.SharedInstance.PlayDontBuyButtonSound();
        StartCoroutine(CloseShopButtonClickedAnimation());
    }

    private IEnumerator CloseShopButtonClickedAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        // Animations
        shopPanelAnimator.SetTrigger("NotActive");
        playButtonAnimator.ResetTrigger("NotActive");
        playButton.gameObject.SetActive(true);
        shopButtonAnimator.ResetTrigger("NotActive");
        shopButton.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(animationTime);

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        shopPanel.gameObject.SetActive(false);
        difficultyPanel.gameObject.SetActive(false);
        buyBackgroundPanel.gameObject.SetActive(false);
        buyTrailPanel.gameObject.SetActive(false);
        clickBlockerPanel.gameObject.SetActive(false);
    }

    public void EasyButtonClicked()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        Sound_Controller.SharedInstance.PlayButtonSound();
        GameData_Controller.SharedInstance.nextGameMode = 0;
        StartCoroutine(ScreenChangerTime());
    }

    public void MediumButtonClicked()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        Sound_Controller.SharedInstance.PlayButtonSound();
        GameData_Controller.SharedInstance.nextGameMode = 1;
        StartCoroutine(ScreenChangerTime());
    }

    public void HardButtonClicked()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        Sound_Controller.SharedInstance.PlayButtonSound();
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