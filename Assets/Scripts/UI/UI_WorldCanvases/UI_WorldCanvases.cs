using System.Collections;
using UnityEngine;

public class UI_WorldCanvases : MonoBehaviour
{
    #region Variables

    [SerializeField] private float animationTime = 1.3f;

    #endregion Variables

    #region Methods

    private void OnEnable()
    {
        StartCoroutine(DeactivateThisGameObject());
    }

    private IEnumerator DeactivateThisGameObject()
    {
        yield return new WaitForSecondsRealtime(animationTime);
        this.gameObject.SetActive(false);
    }

    #endregion Methods
}
// EOF - End Of File