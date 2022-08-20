using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Controller : MonoBehaviour
{
    #region Variables

    public static Shop_Controller SharedInstancel;

    // Backgrounds
    [SerializeField] private int selectedBackground;
    [SerializeField] private int[] backgroundPrices;
    [SerializeField] private GameObject[] backgroundLocks;

    // Trails
    [SerializeField] private int selectedTrail;
    [SerializeField] private int[] trailPrices;
    [SerializeField] private GameObject[] trailLocks;

    // Buy Panels
    [SerializeField] private Animator buyBackgroundPanelAnimator;
    [SerializeField] private Text buyBackgroundPanelText;
    [SerializeField] private Animator buyTrailPanelAnimator;
    [SerializeField] private Text buyTrailPanelText;


    #endregion Variables

    #region Methods

    private void Awake()
    {
        SharedInstancel = this;
    }

    public void InitialiseShopUI()
    {
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
    }

    #region Backgrounds

    public void BackgroundButtonClicked(int buttonIndex)
    {
        if (GameData_Controller.SharedInstance.backgroundsUnlocked[buttonIndex] == true) GameData_Controller.SharedInstance.activeBackground = buttonIndex;   // Setting selected background
        else
        {
            selectedBackground = buttonIndex;
            int tempPrice = backgroundPrices[selectedBackground - 1];
            if(GameData_Controller.SharedInstance.coins > tempPrice) OpenBuyBackgroundPanel(tempPrice);
        }
    }

    #endregion Backgrounds

    #region Trails



    #endregion Trails

    #region BuyPanel

    public void OpenBuyBackgroundPanel(int price)
    {
        buyBackgroundPanelAnimator.SetTrigger("Active");
        buyBackgroundPanelAnimator.ResetTrigger("NotActive");
        buyBackgroundPanelText.text = "Buy this item\nfor: " + price;
    }

    public void BuyBackgroundButtonClicked()
    {
        int tempPrice = backgroundPrices[selectedBackground - 1];
        GameData_Controller.SharedInstance.coins -= tempPrice;
        GameData_Controller.SharedInstance.backgroundsUnlocked[selectedBackground] = true;
        buyBackgroundPanelAnimator.SetTrigger("NotActive");
        buyBackgroundPanelAnimator.ResetTrigger("Active");
        InitialiseShopUI();
        MainMenu_Controller.SharedInstance.UpdateUI();
    }

    #endregion BuyPanel

    #endregion Methods
}
   // EOF - End Of File