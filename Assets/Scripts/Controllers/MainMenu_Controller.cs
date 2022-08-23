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
    [SerializeField] private Text coinText;

    // Buttons
    [SerializeField] private Button playButton, easyButton, mediumButton, hardButton, shopButton, closeShopButton, infoButton;
    [SerializeField] private Animator shopButtonAnimator, infoButtonAnimator;

    // Backgrounds
    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private GameObject clickBlockerPanel;

    // Panels & Animators
    [SerializeField] private GameObject difficultyPanel, shopPanel, buyBackgroundPanel, buyTrailPanel, infoPanel;
    [SerializeField] private Animator playButtonAnimator, difficultyPanelAnimator, shopPanelAnimator, buyBackgroundPanelAnimator, buyTrailPanelAnimator, infoPanelAnimator;

    // Animation Global Time
    [SerializeField] private const float animationTime = 1.1f;

    #endregion Pannels

    #endregion Variables

    #region Methods

    private void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Setting UI Texts
        coinText.text = "" + GameData_Controller.SharedInstance.coins;

        for(int i = 0; i < GameData_Controller.SharedInstance.backgroundsUnlocked.Length; i++)   // Setting the active and selected background, deactivating others
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

        difficultyPanelAnimator.ResetTrigger("NotActive");
        difficultyPanel.gameObject.SetActive(true);   // Activate + Animation
        playButtonAnimator.SetTrigger("NotActive");   // Out Annimation

        yield return new WaitForSecondsRealtime(animationTime);

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        playButton.gameObject.SetActive(false);

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

        playButtonAnimator.ResetTrigger("NotActive");
        playButton.gameObject.SetActive(true);   // Activate + Animation
        difficultyPanelAnimator.SetTrigger("NotActive");   // Out Animation

        yield return new WaitForSecondsRealtime(animationTime);

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        difficultyPanel.gameObject.SetActive(false);

        clickBlockerPanel.gameObject.SetActive(false);
    }

    public void ShopButtonClicked()
    {
        Shop_Controller.SharedInstance.InitialiseShopUI();
        Sound_Controller.SharedInstance.PlayButtonSound();
        StartCoroutine(ShopButtonClickedAnimation());
    }

    private IEnumerator ShopButtonClickedAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);

        shopButtonAnimator.SetTrigger("NotActive");
        if (!shopPanel.gameObject.activeInHierarchy)      // If Not Active -> Activate + Animation
        {
            shopPanelAnimator.ResetTrigger("NotActive");
            shopPanel.gameObject.SetActive(true);
        }
        if (playButton.gameObject.activeInHierarchy) playButtonAnimator.SetTrigger("NotActive");    // If Active -> Out Animation
        if (difficultyPanel.gameObject.activeInHierarchy) difficultyPanelAnimator.SetTrigger("NotActive");

        yield return new WaitForSecondsRealtime(animationTime);

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        shopButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        difficultyPanel.gameObject.SetActive(false);

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

        shopPanelAnimator.SetTrigger("NotActive");   // Out Animation
        if (!playButton.gameObject.activeInHierarchy)   // If Not Active -> Activate + Animation
        {
            playButtonAnimator.ResetTrigger("NotActive");
            playButton.gameObject.SetActive(true);
        }
        if (!shopButton.gameObject.activeInHierarchy)
        {
            shopButtonAnimator.ResetTrigger("NotActive");
            shopButton.gameObject.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(animationTime);

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        shopPanel.gameObject.SetActive(false);
        shopPanelAnimator.ResetTrigger("NotActive");

        clickBlockerPanel.gameObject.SetActive(false);
    }

    public void InfoButtonClicked()
    {
        Sound_Controller.SharedInstance.PlayDontBuyButtonSound();
        StartCoroutine(InfoButtonClickedAnimation());
    }

    private IEnumerator InfoButtonClickedAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);

        infoPanelAnimator.ResetTrigger("NotActive");
        infoPanel.gameObject.SetActive(true);   
        if (infoButton.gameObject.activeInHierarchy) infoButtonAnimator.SetTrigger("NotActive");   // If Active -> Out Animation
        if (playButton.gameObject.activeInHierarchy) playButtonAnimator.SetTrigger("NotActive");   
        if (shopButton.gameObject.activeInHierarchy) shopButtonAnimator.SetTrigger("NotActive");
        if (difficultyPanel.gameObject.activeInHierarchy) difficultyPanelAnimator.SetTrigger("NotActive");
        if (shopPanel.gameObject.activeInHierarchy) shopPanelAnimator.SetTrigger("NotActive");
        if (buyBackgroundPanel.gameObject.activeInHierarchy) buyBackgroundPanelAnimator.SetTrigger("NotActive");
        if (buyTrailPanel.gameObject.activeInHierarchy) buyTrailPanelAnimator.SetTrigger("NotActive");

        yield return new WaitForSecondsRealtime(animationTime);

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        infoButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        difficultyPanel.gameObject.SetActive(false);
        shopPanel.gameObject.SetActive(false);
        buyBackgroundPanel.gameObject.SetActive(false);
        buyTrailPanel.gameObject.SetActive(false);

        clickBlockerPanel.gameObject.SetActive(false);
    }

    public void CloseInfoButtonClicked()
    {
        Sound_Controller.SharedInstance.PlayDontBuyButtonSound();
        StartCoroutine(CloseInfoButtonClickedAnimation());
    }

    private IEnumerator CloseInfoButtonClickedAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);

        // Animations
        infoPanelAnimator.SetTrigger("NotActive");   // Out Animation
        if (!infoButton.gameObject.activeInHierarchy) {   // If Not Active -> Activate
            infoButtonAnimator.ResetTrigger("NotActive");
            infoButton.gameObject.SetActive(true);
        }
        if (!playButton.gameObject.activeInHierarchy) {
            playButtonAnimator.ResetTrigger("NotActive");
            playButton.gameObject.SetActive(true);
        }
        if (!shopButton.gameObject.activeInHierarchy) {
            shopButtonAnimator.ResetTrigger("NotActive");
            shopButton.gameObject.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(animationTime);

        // Deactivating non necessary objects, in order to avoid sorting and functional issues
        infoPanel.gameObject.SetActive(false);
        clickBlockerPanel.gameObject.SetActive(false);
    }

    #region Difficulty Buttons

    public void EasyButtonClicked()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        Sound_Controller.SharedInstance.PlayButtonSound();
        GameData_Controller.SharedInstance.nextGameMode = 0;
        StartCoroutine(ScreenChangerAnimation());
    }

    public void MediumButtonClicked()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        Sound_Controller.SharedInstance.PlayButtonSound();
        GameData_Controller.SharedInstance.nextGameMode = 1;
        StartCoroutine(ScreenChangerAnimation());
    }

    public void HardButtonClicked()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        Sound_Controller.SharedInstance.PlayButtonSound();
        GameData_Controller.SharedInstance.nextGameMode = 2;
        StartCoroutine(ScreenChangerAnimation());
    }

    IEnumerator ScreenChangerAnimation()
    {
        yield return new WaitForSecondsRealtime(1.6f);
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadScene(1);
    }

    #endregion Difficulty Buttons

    #endregion Buttons

    #endregion Methods
}
// EOF - End Of File