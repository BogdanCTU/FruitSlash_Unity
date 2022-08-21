using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameData_Controller : MonoBehaviour
{
    #region Variables

    // Singleton
    public static GameData_Controller SharedInstance;

    // Game Data
    public int nextGameMode = 0;
    public bool isGameStartedFirstTime;

    // Currency
    public int highScore = 0, coins = 0, diamonds = 0;
    private GameData gameData;

    // Backgrounds
    public int activeBackground;
    public bool[] backgroundsUnlocked;

    // Trails
    public int activeTrail;
    public bool[] trailsUnlocked;

    // Sound
    public float soundVolume;

    #endregion

    #region Methods

    private void Awake()
    {
        Singleton();
        InitializeGameVariables();
        Shop_Controller.SharedInstancel.InitialiseShopUI();
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

    public void Save()
    {
        FileStream file = null;

        try   // In order to avoid Exceptions due to IO operations
        {
            BinaryFormatter bf = new BinaryFormatter();
            if (!File.Exists(Application.persistentDataPath + "/GameData.dat"))
            {
                File.Create(Application.persistentDataPath + "/GameData.dat");
            }
            file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
            
            if (gameData != null)
            {
                gameData.SetHighScore(highScore);
                gameData.SetCoins(coins);
                gameData.SetDiamonds(diamonds);
                gameData.SetActiveBackground(activeBackground);
                gameData.SetUnlockedBackgrounds(backgroundsUnlocked);
                gameData.SetActiveTrail(activeTrail);
                gameData.SetUnlockedTrails(trailsUnlocked);
                gameData.SetSoundVolume(soundVolume);

                bf.Serialize(file, gameData);
            }
        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/GameData.dat"))   // Checking if the file exists
        {
            FileStream file = null;

            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
                gameData = (GameData)bf.Deserialize(file);   // Casting
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }
    }

    void InitializeGameVariables()
    {
        Load();

        if (gameData != null) isGameStartedFirstTime = gameData.GetIsGameStartedFirstTime();
        else isGameStartedFirstTime = true;
        
        if (isGameStartedFirstTime)   // Game opened first time
        {
            // Initialising variables
            highScore = 0;
            coins = 0;
            diamonds = 0;

            activeBackground = 0;   // Default Background
            backgroundsUnlocked = new bool[4];
            backgroundsUnlocked[0] = true;   // Default Background
            for (int i = 1; i < backgroundsUnlocked.Length; i++)
            {
                backgroundsUnlocked[i] = false;
            }

            activeTrail = 0;   // Default Trail
            trailsUnlocked = new bool[4];
            trailsUnlocked[0] = true;   // Default Trail
            for (int i = 1; i < trailsUnlocked.Length; i++)
            {
                trailsUnlocked[i] = false;
            }

            soundVolume = 1.0f;

            // Setting data
            gameData = new GameData();
            gameData.SetHighScore(highScore);
            gameData.SetCoins(coins);
            gameData.SetDiamonds(diamonds);
            gameData.SetActiveBackground(activeBackground);
            gameData.SetUnlockedBackgrounds(backgroundsUnlocked);
            gameData.SetActiveTrail(activeTrail);
            gameData.SetUnlockedTrails(trailsUnlocked);
            gameData.SetSoundVolume(soundVolume);

            // Saving Data
            Save();
        }
        else   // Game already played, load Data
        {
            highScore = gameData.GetHighScore();
            coins = gameData.GetCoins();
            diamonds = gameData.GetDiamonds();
            activeBackground = gameData.GetActiveBackground();
            backgroundsUnlocked = gameData.GetUnlockedBackgrounds();
            activeTrail = gameData.GetActiveTrail();
            trailsUnlocked = gameData.GetUnlockedTrails();
            soundVolume = gameData.GetSoundVolume();
        }
    }

    #endregion
}

[Serializable]
class GameData
{
    #region Variables

    // Game Data
    private bool isGameStartedFirstTime;

    // Currency
    private int highScore, coins, diamonds;

    // Backgrounds
    public int activeBackground;
    public bool[] backgroundsUnlocked;

    // Trails
    public int activeTrail;
    public bool[] trailsUnlocked;

    // Sound
    public float soundVolume;

    # endregion Variables

    #region Getters and Setters

    public void SetIsGameStartedFirstTime(bool isGameStartedFirstTime) { this.isGameStartedFirstTime = isGameStartedFirstTime; }
    public bool GetIsGameStartedFirstTime() { return this.isGameStartedFirstTime; }

    public void SetHighScore(int highScore) { this.highScore = highScore; }
    public int GetHighScore() { return this.highScore; }

    public void SetCoins(int coins) { this.coins = coins; }
    public int GetCoins() { return this.coins; }

    public void SetDiamonds(int diamonds) { this.diamonds = diamonds; }
    public int GetDiamonds() { return this.diamonds; }

    public void SetActiveBackground(int background) { this.activeBackground = background; }
    public int GetActiveBackground() { return this.activeBackground; }

    public void SetActiveTrail(int trail) { this.activeTrail = trail; }
    public int GetActiveTrail() { return this.activeTrail; }

    public void SetUnlockedBackgrounds(bool[] backgrounds) { this.backgroundsUnlocked = backgrounds; }
    public bool[] GetUnlockedBackgrounds() { return this.backgroundsUnlocked; }

    public void SetUnlockedTrails(bool[] trails) { this.trailsUnlocked = trails; }
    public bool[] GetUnlockedTrails() { return this.trailsUnlocked; }

    public void SetSoundVolume(float volume) { this.soundVolume = volume; }
    public float GetSoundVolume() { return this.soundVolume; }

    #endregion Getters and Setters
}

// EOF - End Of File