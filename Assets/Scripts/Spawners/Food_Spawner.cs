using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   // Allows to make instances of this class editable from within the Inspector.
public class Food_Spawner : MonoBehaviour
{
    #region Variables

    #region PoolingMethod
    // Shared Instance
    public static Food_Spawner SharedInstance;

    // Pooling List
    [SerializeField]
    private List<GameObject> pooledObjects;

    // ObjectsToPool Class
    [SerializeField]
    private List<ObjectPoolItem> itemsToPool;
    #endregion

    // Object to pull tag and type
    protected string objectTag;    // objectTag  {cookie, steak, pizza}
    protected int objectType;      // objectType {  0   ,   1  ,   2  }

    // Timers
    [SerializeField]
    protected float nextObjectTimerMin = 2.0f, nextObjectTimerMax = 3.0f;
    protected float nextObjectTimer;

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

    #region Object Pooling Methods
    protected void SetObjectsToPull()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    protected GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
    #endregion

    protected IEnumerator SpawnObject()
    {
        Debug.Log("Coroutine - started!");

        RandomFoodType();   // Randomise food to spawn

        GameObject objectToSpawn = GetPooledObject(objectTag);
        if (objectToSpawn != null)
        {
            objectToSpawn.SetActive(true);
            objectToSpawn.GetComponent<Food>().SetPosition();
            objectToSpawn.GetComponent<Food>().MoveObject();
        }

        // Code will be executer before starting timer
        yield return new WaitForSecondsRealtime(nextObjectTimer);
        // Code will be executed after time is over

        nextObjectTimer = Random.Range(nextObjectTimerMin, nextObjectTimerMax);
        StartCoroutine("SpawnObject");
    }

    private void RandomFoodType()
    {
        // Randomise FoodType to spawn
        objectType = Random.Range(0, 3);   // The max value is excluded
        if (objectType == 0) objectTag = "Cookie";
        else if (objectType == 1) objectTag = "Steak";
        else if (objectType == 2) objectTag = "Pizza";
    }

    #region Getters/Setters

    public void SetMinSpawnObjectTime(float minTime) { this.nextObjectTimerMin = minTime; }
    public float GetMinSpawnObjectTime() { return this.nextObjectTimerMin; }

    public void SetMaxSpawnObjectTime(float maxTime) { this.nextObjectTimerMax = maxTime; }
    public float GetMaxSpawnObjectTime() { return this.nextObjectTimerMax; }

    #endregion

    #endregion
}

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;
    public GameObject objectToPool;
    public bool shouldExpand;
}

// EOF - End Of File