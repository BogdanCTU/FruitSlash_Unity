using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Controller : MonoBehaviour
{
    #region Variables

    public static Shop_Controller SharedInstancel;

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

    #endregion Variables

    #region Methods

    private void Awake()
    {
        SharedInstancel = this;
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
        // Currency
        int tempPrice = backgroundPrices[selectedBackground - 1];
        GameData_Controller.SharedInstance.coins -= tempPrice;
        GameData_Controller.SharedInstance.backgroundsUnlocked[selectedBackground] = true;

        StartCoroutine(CloseBuyBackgroundPanelAnimation());

        // Updating UI
        InitialiseShopUI();
        MainMenu_Controller.SharedInstance.UpdateUI();
    }

    private IEnumerator CloseBuyBackgroundPanelAnimation()
    {
        buyBackgroundPanelAnimator.SetTrigger("NotActive");
        yield return new WaitForSecondsRealtime(animationTime);
        buyBackgroundPanel.gameObject.SetActive(false);
    }

    #endregion Backgrounds

    #region Trails

    public void TrailButtonClicked(int buttonIndex)
    {
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
        buyTrailPanelAnimator.SetTrigger("Active");
        buyTrailPanelAnimator.ResetTrigger("NotActive");
        buyTrailPanelText.text = "Buy this item\nfor: " + price;
    }

    public void BuyTrailButtonClicked()   // Confirm trail purchase and updating UI
    {
        // Currency
        int tempPrice = trailPrices[selectedTrail - 1];
        GameData_Controller.SharedInstance.coins -= tempPrice;
        GameData_Controller.SharedInstance.trailsUnlocked[selectedTrail] = true;

        // Panels
        buyTrailPanelAnimator.SetTrigger("NotActive");
        buyTrailPanelAnimator.ResetTrigger("Active");

        // Updating UI
        InitialiseShopUI();
        MainMenu_Controller.SharedInstance.UpdateUI();
    }

    #endregion Trails

    #endregion Methods
}
// EOF - End Of File