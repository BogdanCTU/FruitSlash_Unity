using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound_Button : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject redBackground, greenBackground, xImage;
    [SerializeField] private Animator buttonClickAnimator;

    #endregion Variables

    #region Methods

    public void soundButtonClicked()
    {
        buttonClickAnimator.Play("SoundButtonClick");
        if (Sound_Controller.SharedInstance.audioActive == true)
        {
            Sound_Controller.SharedInstance.MuteSound();
            Sound_Controller.SharedInstance.audioActive = false;
            greenBackground.gameObject.SetActive(false);
            redBackground.gameObject.SetActive(true);
            xImage.gameObject.SetActive(true);
        }
        else if (!Sound_Controller.SharedInstance.audioActive)
        {
            Sound_Controller.SharedInstance.UnMuteSound();
            Sound_Controller.SharedInstance.audioActive = true;
            greenBackground.gameObject.SetActive(true);
            redBackground.gameObject.SetActive(false);
            xImage.gameObject.SetActive(false);
        }
        else
        {
            Sound_Controller.SharedInstance.MuteSound();
            Sound_Controller.SharedInstance.audioActive = false;
            greenBackground.gameObject.SetActive(false);
            redBackground.gameObject.SetActive(true);
            xImage.gameObject.SetActive(true);
        }
    }

    #endregion Methods
}
// EOF - End Of File