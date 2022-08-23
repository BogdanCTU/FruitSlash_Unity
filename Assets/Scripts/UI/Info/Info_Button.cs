using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    #region Variables

    [SerializeField] private Button infoButton;
    [SerializeField] private Animator infoButtonAnimator;
    [SerializeField] private float animationTime = 1.1f;

    #endregion Variables

    #region Methods

    public void InfoButtonClicked()
    {
        StartCoroutine(InfoButtonClickedAnimation());
    }

    private IEnumerator InfoButtonClickedAnimation()
    {
        infoButtonAnimator.SetTrigger("NotActive");   // Playing Animation
        yield return new WaitForSecondsRealtime(animationTime);
        infoButtonAnimator.ResetTrigger("NotActive");   // Resetting Animation
        infoButton.gameObject.SetActive(false);
    }

    #endregion Methods
}
