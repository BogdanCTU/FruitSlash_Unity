using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    #region Variables

    // Singleton
    public static Game_Controller SharedInstance;

    // Game Data
    public int highScore, coind, lives;
    public bool isGameStartedFirstTime = true;
    private GameData gameData;

    //[SerializeField]
    //private GameObject mouseSprite;

    #endregion

    #region Methods

    private void Awake()
    {
        Singleton();
        InitializeGameVariables();
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
            file = File.Create(Application.persistentDataPath + "/GameData.dat");

            if (gameData != null)
            {
                gameData.SetHighScore(highScore);
                bf.Serialize(file, gameData);
            }
        }
        catch (Exception e)
        {
            // Nothing
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
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
            gameData = (GameData)bf.Deserialize(file);   // Casting
        }
        catch (Exception e)
        {
            // Nothing
        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    void InitializeGameVariables()
    {
        Load();

        if (gameData != null)
        {
            isGameStartedFirstTime = gameData.GetIsGameStartedFirstTime();
        }
        else
        {
            isGameStartedFirstTime = true;
        }

        if (isGameStartedFirstTime)   // Game opened first time
        {
            // Initialising variables
            highScore = 0;

            // Setting data
            gameData = new GameData();
            gameData.SetHighScore(highScore);

            // Saving Data
            Save();
            Load();
        }
        else
        {
            highScore = gameData.GetHighScore();
        }
    }

    #endregion
}

class GameData
{
    #region Global Variables

    private int highScore;
    private bool isGameStartedFirstTime;

    # endregion

    #region Getters and Setters

    public void SetIsGameStartedFirstTime(bool isGameStartedFirstTime) { this.isGameStartedFirstTime = isGameStartedFirstTime; }
    public bool GetIsGameStartedFirstTime() { return this.isGameStartedFirstTime; }

    public void SetHighScore(int highScore) { this.highScore = highScore; }
    public int GetHighScore() { return this.highScore; }

    #endregion
}

   // EOF - End Of File