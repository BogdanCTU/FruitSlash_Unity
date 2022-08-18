using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.GraphicsBuffer;

public class Particle_Spawner : MonoBehaviour
{
    #region Variables

    // Shared Instance
    public static Particle_Spawner SharedInstance;

    #region Pooling Method

    // Pooling Method Variables
    [SerializeField] private List<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;
    [SerializeField] private bool shouldExpand = true;

    #endregion Pooling Method

    #endregion Variables

    #region Methods

    #region Unity Methods

    protected void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;

        SetObjectsToPull();
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

    #endregion Methods
}
   // EOF - End Of File