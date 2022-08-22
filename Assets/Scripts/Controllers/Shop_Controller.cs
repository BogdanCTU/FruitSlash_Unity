using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Controller : MonoBehaviour
{
    #region Variables

    public static Shop_Controller SharedInstance;

    // Backgrounds Elements
    [SerializeField] private int selectedBackground;
    [SerializeField] private int[] backgroundPrices;
    [SerializeField] private GameObject[] backgroundLocks;   // Lock Images
    [SerializeField] private GameObject[] backgroundSelecteds;   // Selected Background Image
    [SerializeField] private GameObject buyBackgroundPanel;
    [SerializeField] private Animator buyBackgroundPanelAnimator;
    [SerializeField] private Text buyBackgroundPanelText;

    // Trails Elements
    [SerializeField] private int selectedTrail;
    [SerializeField] private int[] trailPrices;
    [SerializeField] private GameObject[] trailLocks;   // Lock Images
    [SerializeField] private GameObject[] trailSelecteds;   // Selected Trail Image
    [SerializeField] private GameObject buyTrailPanel;
    [SerializeField] private Animator buyTrailPanelAnimator;
    [SerializeField] private Text buyTrailPanelText;

    [SerializeField] private const float animationTime = 1.1f;

    // Click Blocker
    [SerializeField] private GameObject clickBlockerPanel;

    #endregion Variables

    #region Methods

    private void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;
    }

    public void InitialiseShopUI()
    {
        // Lock Images
        for(int i = 1; i < GameData_Controller.SharedInstance.backgroundsUnlocked.Length; i++)
        {
            if (GameData_Controller.SharedInstance.backgroundsUnlocked[i] == true) backgroundLocks[i-1].gameObject.SetActive(false);
            else backgroundLocks[i - 1].gameObject.SetActive(true);
        }
        for (int i = 1; i < GameData_Controller.SharedInstance.trailsUnlocked.Length; i++)
        {
            if (GameData_Controller.SharedInstance.trailsUnlocked[i] == true) trailLocks[i - 1].gameObject.SetActive(false);
            else trailLocks[i - 1].gameObject.SetActive(true);
        }
        
        // Selected Images
        for (int i = 0; i < backgroundSelecteds.Length; i++)   // Desactivating non active backgrounds images
        {
            if (i == GameData_Controller.SharedInstance.activeBackground) backgroundSelecteds[i].gameObject.SetActive(true);
            else backgroundSelecteds[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < trailSelecteds.Length; i++)   // Desactivating non active trails images 
        {
            if (i == GameData_Controller.SharedInstance.activeTrail) trailSelecteds[i].gameObject.SetActive(true);
            else trailSelecteds[i].gameObject.SetActive(false);
        }
    }

    #region Backgrounds

    public void BackgroundButtonClicked(int buttonIndex)
    {
        Sound_Controller.SharedInstance.PlayButtonSound();
        if (GameData_Controller.SharedInstance.backgroundsUnlocked[buttonIndex] == true)
        {
            GameData_Controller.SharedInstance.activeBackground = buttonIndex;   // Setting selected background
            InitialiseShopUI();
            MainMenu_Controller.SharedInstance.UpdateUI();
        }
        else
        {
            selectedBackground = buttonIndex;
            int tempPrice = backgroundPrices[selectedBackground - 1];
            if (GameData_Controller.SharedInstance.coins > tempPrice) OpenBuyBackgroundPanel(tempPrice);
        }
    }

    public void OpenBuyBackgroundPanel(int price)   // Which button was pressed
    {
        buyBackgroundPanelAnimator.ResetTrigger("NotActive");
        buyBackgroundPanel.gameObject.SetActive(true);
        buyBackgroundPanelText.text = "Buy this item\nfor: " + price;
    }

    public void BuyBackgroundButtonClicked()   // Confirm background purchase and updating UI
    {
        Sound_Controller.SharedInstance.PlayBuyButtonSound();
        // Currency
        int tempPrice = backgroundPrices[selectedBackground - 1];
        GameData_Controller.SharedInstance.coins -= tempPrice;
        GameData_Controller.SharedInstance.backgroundsUnlocked[selectedBackground] = true;

        StartCoroutine(CloseBuyBackgroundPanelAnimation());

        // Updating UI
        InitialiseShopUI();
        MainMenu_Controller.SharedInstance.UpdateUI();
        GameData_Controller.SharedInstance.Save();   // Saving New Data
    }

    public void CloseBackgroundButtonClicked()   // Confirm background purchase and updating UI
    {
        Sound_Controller.SharedInstance.PlayDontBuyButtonSound();
        StartCoroutine(CloseBuyBackgroundPanelAnimation());

        // Updating UI
        InitialiseShopUI();
        MainMenu_Controller.SharedInstance.UpdateUI();
    }

    private IEnumerator CloseBuyBackgroundPanelAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        buyBackgroundPanelAnimator.SetTrigger("NotActive");
        yield return new WaitForSecondsRealtime(animationTime);
        buyBackgroundPanel.gameObject.SetActive(false);
        clickBlockerPanel.gameObject.SetActive(false);
    }

    #endregion Backgrounds

    #region Trails

    public void TrailButtonClicked(int buttonIndex)
    {
        Sound_Controller.SharedInstance.PlayButtonSound();
        if (GameData_Controller.SharedInstance.trailsUnlocked[buttonIndex] == true)
        {
            GameData_Controller.SharedInstance.activeTrail = buttonIndex;   // Setting selected background
            InitialiseShopUI();
            MainMenu_Controller.SharedInstance.UpdateUI();
        }
        else
        {
            selectedTrail = buttonIndex;
            int tempPrice = trailPrices[selectedTrail - 1];
            if (GameData_Controller.SharedInstance.coins > tempPrice) OpenBuyTrailPanel(tempPrice);
        }
    }

    public void OpenBuyTrailPanel(int price)   // Which button was pressed
    {
        buyTrailPanelAnimator.ResetTrigger("NotActive");
        buyTrailPanel.gameObject.SetActive(true);
        buyTrailPanelText.text = "Buy this item\nfor: " + price;
    }

    public void BuyTrailButtonClicked()   // Confirm trail purchase and updating UI
    {
        Sound_Controller.SharedInstance.PlayBuyButtonSound();
        // Currency
        int tempPrice = trailPrices[selectedTrail - 1];
        GameData_Controller.SharedInstance.coins -= tempPrice;
        GameData_Controller.SharedInstance.trailsUnlocked[selectedTrail] = true;

        StartCoroutine(CloseBuyTrailPanelAnimation());
        
        // Updating UI
        InitialiseShopUI();
        MainMenu_Controller.SharedInstance.UpdateUI();
        GameData_Controller.SharedInstance.Save();   // Saving New Data
    }

    public void CloseTrailButtonClicked()   // Confirm background purchase and updating UI
    {
        Sound_Controller.SharedInstance.PlayDontBuyButtonSound();
        StartCoroutine(CloseBuyTrailPanelAnimation());

        // Updating UI
        InitialiseShopUI();
        MainMenu_Controller.SharedInstance.UpdateUI();
    }

    private IEnumerator CloseBuyTrailPanelAnimation()
    {
        clickBlockerPanel.gameObject.SetActive(true);
        buyTrailPanelAnimator.SetTrigger("NotActive");
        yield return new WaitForSecondsRealtime(animationTime);
        buyTrailPanel.gameObject.SetActive(false);
        clickBlockerPanel.gameObject.SetActive(false);
    }

    #endregion Trails

    #endregion Methods
}
// EOF - End Of File