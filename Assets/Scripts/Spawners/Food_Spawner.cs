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
    [SerializeField] private List<GameObject> pooledObjects;

    // ObjectsToPool Class
    [SerializeField] private List<ObjectPoolItem> itemsToPool;

    #endregion

    // Fruits
    [SerializeField] private int fruitsNumber;

    // Timers
    [SerializeField] private float nextObjectTimerMin = 2.0f, nextObjectTimerMax = 3.0f;
    private float nextObjectTimer;

    #endregion Variables

    #region Methods

    #region Unity Methods

    protected void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;

        nextObjectTimer = Random.Range(nextObjectTimerMin, nextObjectTimerMax);   // Setting First Timer

        SetObjectsToPull();
        StartCoroutine(SpawnObject());
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnObject());
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
        string objectTag = RandomFoodType();    // objectTag  {Fruit1 - Apple, Fruit2 - Coconut, Fruit3 - Lemon, Fruit4 - Orange, Fruit5 - Pear}

        GameObject objectToSpawn = GetPooledObject(objectTag);
        if (objectToSpawn != null)
        {
            objectToSpawn.SetActive(true);
        }

        // Code will be executer before starting timer
        yield return new WaitForSecondsRealtime(nextObjectTimer);
        // Code will be executed after time is over

        nextObjectTimer = Random.Range(nextObjectTimerMin, nextObjectTimerMax);
        StartCoroutine(SpawnObject());
    }

    private string RandomFoodType()
    {
        // Randomise FoodType to spawn
        int objectType = Random.Range(1, fruitsNumber + 1);   // The max value is excluded
        string fruitName = "Fruit" + objectType;
        return fruitName;
    }

    #endregion Methods
}

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;
    public GameObject objectToPool;
    public bool shouldExpand;
}

// EOF - End Of File