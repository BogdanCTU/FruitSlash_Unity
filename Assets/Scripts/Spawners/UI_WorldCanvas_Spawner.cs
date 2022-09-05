using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WorldCanvas_Spawner : MonoBehaviour
{
    #region Variables

    #region PoolingMethod

    // Shared Instance
    public static UI_WorldCanvas_Spawner SharedInstance;

    // Pooling List
    [SerializeField] private List<GameObject> pooledObjects;

    // ObjectsToPool Class
    [SerializeField] private List<ObjectPoolItem> itemsToPool;

    #endregion

    #endregion Variables

    #region Methods

    #region Unity Methods

    protected void Awake()
    {
        if (SharedInstance == null) SharedInstance = this;

        SetObjectsToPull();
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

    #endregion Object Pooling Methods

    public void SpawnLifeObject(Vector3 parentPosition)
    {
        GameObject objectToSpawn = GetPooledObject("LifeCanvas");
        if (objectToSpawn != null)
        {
            objectToSpawn.gameObject.transform.position = parentPosition;
            objectToSpawn.SetActive(true);
        }
    }

    public void SpawnCoinObject(Vector3 parentPosition, int parentPoints)
    {
        int multiplier = Gameplay_Controller.SharedInstance.GetGameMode() + 1;   // Setting multiplier based on Game Difficutly

        GameObject objectToSpawn = GetPooledObject("CoinCanvas");
        if (objectToSpawn != null)
        {
            // Setting WorldCanvas Text based on FruitType
            Text canvasText = objectToSpawn.GetComponentInChildren(typeof(Text)) as Text;
            canvasText.text = "+" + (parentPoints * multiplier);
            objectToSpawn.gameObject.transform.position = new Vector3(parentPosition.x, parentPosition.y, parentPosition.z - 1);
            objectToSpawn.SetActive(true);
        }
    }

    public void SpawnComboObject(Vector3 parentPosition, int combo)
    {
        if (combo >= 2)
        {
            Gameplay_Controller.SharedInstance.actualScore += combo;   // Adding COMBO bonus points
            GameObject objectToSpawn = GetPooledObject("ComboCanvas");
            Text canvasText = objectToSpawn.GetComponentInChildren(typeof(Text)) as Text;
            if (objectToSpawn != null)
            {
                canvasText.text = "COMBO X" + combo;
                objectToSpawn.gameObject.transform.position = new Vector3(parentPosition.x, parentPosition.y, parentPosition.z - 1);
                objectToSpawn.SetActive(true);
            }
        }
    }

    #endregion Methods
}

// EOF - End Of File