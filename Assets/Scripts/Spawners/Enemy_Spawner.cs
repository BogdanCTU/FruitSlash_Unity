using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.GraphicsBuffer;

public class Enemy_Spawner : MonoBehaviour
{
    #region Variables

    // Shared Instance
    public static Enemy_Spawner SharedInstance;

    #region Pooling Method

    // Pooling Method Variables
    [SerializeField] private List<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;
    [SerializeField] private bool shouldExpand = true;

    // Timers
    [SerializeField]
    private float nextObjectTimerMin = 2.0f, nextObjectTimerMax = 3.0f;
    private float nextObjectTimer;

    #endregion Pooling Method

    #endregion

    #region Methods

    #region Unity Methods

    protected void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;

        // Setting Timer
        nextObjectTimer = Random.Range(nextObjectTimerMin, nextObjectTimerMax);

        SetObjectsToPull();
        StartCoroutine("SpawnObject");
    }

    private void OnEnable()
    {
        StartCoroutine("SpawnObject");
    }

    #endregion Unity Methods

    #region Object Pooling Method

    protected void SetObjectsToPull()
    {
        pooledObjects = new List<GameObject>();
        GameObject gameObjectTmp;
        for (int i = 0; i < amountToPool; i++)
        {
            gameObjectTmp = Instantiate(objectToPool);
            gameObjectTmp.SetActive(false);
            pooledObjects.Add(gameObjectTmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        if (shouldExpand)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }
        else
        {
            return null;
        }
    }

    #endregion Object Pooling Method

    protected IEnumerator SpawnObject()
    {
        Debug.Log("Coroutine - started!");
        GameObject gameObject = Enemy_Spawner.SharedInstance.GetPooledObject();
        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }

        // Code will be executer before starting timer
        yield return new WaitForSecondsRealtime(nextObjectTimer);
        // Code will be executed after time is over

        nextObjectTimer = Random.Range(nextObjectTimerMin, nextObjectTimerMax);
        StartCoroutine("SpawnObject");
    }

    #region Getters/Setters

    public void SetMinSpawnObjectTime(float minTime) { this.nextObjectTimerMin = minTime; }
    public float GetMinSpawnObjectTime() { return this.nextObjectTimerMin; }

    public void SetMaxSpawnObjectTime(float maxTime) { this.nextObjectTimerMax = maxTime; }
    public float GetMaxSpawnObjectTime() { return this.nextObjectTimerMax; }

    #endregion

    #endregion
}
   // EOF - End Of File