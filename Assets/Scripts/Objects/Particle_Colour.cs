using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Colour : MonoBehaviour
{
    #region Variables

    [SerializeField] private ParticleSystem _particleSystem;

    #endregion

    #region Methods
    
    private void OnEnable()
    {
        _particleSystem.Play();
        StartCoroutine(DisableEffect());
    }

    IEnumerator DisableEffect()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

    #endregion
}
