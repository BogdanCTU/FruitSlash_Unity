using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Controller : MonoBehaviour
{
    #region Variables

    public static Sound_Controller SharedInstance;

    // Button Sounds
    public AudioSource buttonSoundSource, buyButtonSoundSource, dontBuyButtonSoundSource;

    // Fruit Sounds
    public AudioSource[] fruitAudioSource;

    // Enemy Sounds
    public AudioSource[] enemyAudioSource;

    // Background Music

    #endregion Variables

    #region Methods

    private void Awake()
    {
        Singleton();
    }

    public void Singleton()   // Singleton class, only one instance
    {
        if (SharedInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            SharedInstance = this;
            DontDestroyOnLoad(gameObject);   // Singleton Class, preserve the game object
                                             // between scenes because there can be only one
        }
    }

    public void PlayButtonSound()
    {
        buttonSoundSource.Play();
    }

    public void PlayBuyButtonSound()
    {
        buyButtonSoundSource.Play();
    }

    public void PlayDontBuyButtonSound()
    {
        dontBuyButtonSoundSource.Play();
    }

    public void PlayFruitSound()
    {
        fruitAudioSource[Random.Range(0, 9)].Play();   // 8 random sounds to play
    }

    public void PlayEnemySound()
    {
        //enemyAudioSource[Random.Range(0, 3)].Play();   // 3 random sounds to play
    }

    #endregion Methods
}
   // EOF - End Of File