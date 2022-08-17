using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   // Allows to make instances of this class editable from within the Inspector.
public class Spawner_Controller : MonoBehaviour
{
    #region Variables

    #region PoolingMethod
    // Shared Instance
    public static Spawner_Controller SharedInstance;

    // Pooling List
    [SerializeField]
    private List<GameObject> pooledObjects;

    // ObjectsToPool Class
    [SerializeField]
    private List<ObjectPoolItem> itemsToPool;
    #endregion

    //Object to pull tag
    [SerializeField]
    protected string objectTag = "Skull";

    // Timers
    [SerializeField]
    protected float nextObjectTimerMin = 2.0f, nextObjectTimerMax = 3.0f;
    protected float nextObjectTimer;

    #endregion

    #region Methods

    protected void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;

        // Setting Timer
        nextObjectTimer = Random.Range(nextObjectTimerMin, nextObjectTimerMax);

        SetObjectsToPull();
        StartCoroutine("SpawnObject");
    }

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
        GameObject skull = GetPooledObject(objectTag);
        if (skull != null)
        {
            skull.SetActive(true);
            skull.GetComponent<Skull>().SetPosition();
            skull.GetComponent<Skull>().MoveObject();
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

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;   // The starting ammount of objects to pull (it will increase eventually)
    public GameObject objectToPool;   // GameObject to pull
    public bool shouldExpand;   // Checking if more objects are needed
}
   // EOF - End Of File