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
    
    // Spawn Sound
    public AudioSource spawnSound;

    // Background Music
    public AudioSource backgroundAudioSource;

    // Volume
    public float backgroundVolume = 0.5f;
    public float audioSourceVolume = 1.0f;
    public bool audioActive = true;

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
        enemyAudioSource[Random.Range(0, 3)].Play();   // 3 random sounds to play
    }

    public void PlaySpawnSound()
    {
        spawnSound.Play();
    }

    public void MuteSound()
    {
        backgroundVolume = 0f;
        audioSourceVolume = 0f;

        backgroundAudioSource.volume = backgroundVolume;
        for(int i = 0; i < fruitAudioSource.Length; i++)
        {
            fruitAudioSource[i].volume = 0;
        }
        for (int i = 0; i < enemyAudioSource.Length; i++)
        {
            enemyAudioSource[i].volume = 0;
        }
        spawnSound.volume = 0;
        buttonSoundSource.volume = audioSourceVolume;
        buyButtonSoundSource.volume = audioSourceVolume;
        dontBuyButtonSoundSource.volume = audioSourceVolume;
    }

    public void UnMuteSound()
    {
        backgroundVolume = 0.5f;
        audioSourceVolume = 1.0f;

        backgroundAudioSource.volume = backgroundVolume;
        for (int i = 0; i < fruitAudioSource.Length; i++)
        {
            fruitAudioSource[i].volume = audioSourceVolume;
        }
        for (int i = 0; i < enemyAudioSource.Length; i++)
        {
            enemyAudioSource[i].volume = audioSourceVolume;
        }
        spawnSound.volume = audioSourceVolume;
        buttonSoundSource.volume = audioSourceVolume; 
        buyButtonSoundSource.volume = audioSourceVolume; 
        dontBuyButtonSoundSource.volume = audioSourceVolume;
    }

    #endregion Methods
}
// EOF - End Of File