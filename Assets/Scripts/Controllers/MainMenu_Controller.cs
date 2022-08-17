using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_Controller : MonoBehaviour
{
    #region Variables

    // Shared Instance
    public static MainMenu_Controller SharedInstance;

    #region Pannels

    [SerializeField]
    private Canvas mainMenuCanvas, gamePlayCanvas;

    [SerializeField]
    private Animator difficultyPanelAnimator;
    private bool difficultyPanelHidden = true;

    [SerializeField]
    private Button playButton;

    #endregion

    #endregion

    #region Methods

    private void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;
    }

    #region Buttons

    public void PlayButtonClicked()
    {
        playButton.gameObject.GetComponent<Button>().enabled = false;
        if (difficultyPanelHidden)
        {
            difficultyPanelAnimator.Play("DifficultyPanelZoomIn");
        }
        else
        {
            difficultyPanelAnimator.Play("DifficultyPanelZoomOut");
        }
    }

    public void EasyButtonClicked()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        gamePlayCanvas.gameObject.SetActive(true);
        Gameplay_Controller.SharedInstance.SetGameMode(0);
        Gameplay_Controller.SharedInstance.InitialiseGameplay();
    }

    public void MediumButtonClicked()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        gamePlayCanvas.gameObject.SetActive(true);
        Gameplay_Controller.SharedInstance.SetGameMode(1);
        Gameplay_Controller.SharedInstance.InitialiseGameplay();
    }

    public void HardButtonClicked()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        gamePlayCanvas.gameObject.SetActive(true);
        Gameplay_Controller.SharedInstance.SetGameMode(2);
        Gameplay_Controller.SharedInstance.InitialiseGameplay();
    }

    public void MainMenuSetActiveTrue()
    {
        mainMenuCanvas.gameObject.SetActive(true);
        gamePlayCanvas.gameObject.SetActive(false);
        playButton.gameObject.GetComponent<Button>().enabled = true;
    }

    #endregion

    #endregion
}
// EOF - End Of File