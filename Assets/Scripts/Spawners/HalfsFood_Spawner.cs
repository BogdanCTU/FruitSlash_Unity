using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class HalfsFood_Spawner : MonoBehaviour
{
    #region Variables

    // Shared Instance
    public static HalfsFood_Spawner SharedInstance;

    #region Pooling Method

    // Pooling Method Variables
    [SerializeField] private List<GameObject> pooledObjects;   // Pooling List
    [SerializeField] private List<ObjectPoolItem> itemsToPool;   // ObjectsToPool Class

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

    public void SpawnRightHalfFruit(Vector3 direction, Vector3 parentPosition, string parentTag)
    {
        string gameObjectTag = "";
        GameObject rightHalfFruit;
        switch (parentTag)
        {
            case "Fruit1":
                {
                    gameObjectTag = "HalfApple";
                    rightHalfFruit = GetPooledObject(gameObjectTag);
                    if (rightHalfFruit != null)
                    {
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;   // Converting fron Radius to Degrees
                        //Debug.Log("BEFORE A: " + angle);
                        angle = angle <= -90f && angle >= -180f ? ((angle * -1f) - 90) : angle;
                        angle = angle <= -180f && angle >= -270f ? (angle + 180) : angle;
                        //Debug.Log("AFTER A: " + angle);
                        rightHalfFruit.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                        rightHalfFruit.transform.position = new Vector3(parentPosition.x, parentPosition.y, 0);
                        rightHalfFruit.SetActive(true);
                    }
                    break;
                }
            case "Fruit2":
                {
                    gameObjectTag = "HalfCoconut";
                    rightHalfFruit = GetPooledObject(gameObjectTag);
                    if (rightHalfFruit != null)
                    {
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;   // Converting fron Radius to Degrees
                        Debug.Log("BEFORE A: " + angle);
                        angle = angle < 0f && angle > -90 ? ((angle * -1f) + 90) : angle;
                        angle = angle <= -90f && angle > -180 ? ((angle * -1f) - 90) : angle;
                        Debug.Log("AFTER A: " + angle);
                        rightHalfFruit.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                        rightHalfFruit.transform.position = new Vector3(parentPosition.x, parentPosition.y, 0);
                        rightHalfFruit.SetActive(true);
                    }
                    break;
                }
            case "Fruit3":
                {
                    gameObjectTag = "HalfLemon";
                    rightHalfFruit = GetPooledObject(gameObjectTag);
                    if (rightHalfFruit != null)
                    {
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;   // Converting fron Radius to Degrees
                        rightHalfFruit.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                        rightHalfFruit.transform.position = new Vector3(parentPosition.x, parentPosition.y, 0);
                        rightHalfFruit.SetActive(true);
                    }
                    break;
                }
        }
    }

    public void SpawnLeftHalfFruit(Vector3 direction, Vector3 parentPosition, string parentTag)
    {
        string gameObjectTag = "";
        GameObject leftHalfFruit;
        switch (parentTag)
        {
            case "Fruit1":
                {
                    gameObjectTag = "HalfApple";
                    leftHalfFruit = GetPooledObject(gameObjectTag);
                    if (leftHalfFruit != null)
                    {
                        leftHalfFruit.GetComponent<FruitHalf>().fallRight = false;
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;   // Converting fron Radius to Degrees
                        angle = angle <= -90.1f && angle >= -179.9f ? ((angle * -1f) - 90) : angle;
                        angle = angle <= -180.1f && angle >= -269.9f ? (angle + 180) : angle;
                        leftHalfFruit.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                        leftHalfFruit.transform.localScale = new Vector3(-1f, 1f, 1f);
                        leftHalfFruit.transform.position = new Vector3(parentPosition.x, parentPosition.y, 0);
                        leftHalfFruit.SetActive(true);
                    }
                    break;
                }
            case "Fruit2":
                {
                    gameObjectTag = "HalfCoconut";
                    leftHalfFruit = GetPooledObject(gameObjectTag);
                    if (leftHalfFruit != null)
                    {
                        leftHalfFruit.GetComponent<FruitHalf>().fallRight = false;
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;   // Converting fron Radius to Degrees
                        leftHalfFruit.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                        angle = angle < 0f && angle > -90 ? ((angle * -1f) + 90) : angle;
                        angle = angle <= -90f && angle > -180 ? ((angle * -1f) - 90) : angle;
                        leftHalfFruit.transform.localScale = new Vector3(1f, -1f, 1f);
                        leftHalfFruit.transform.position = new Vector3(parentPosition.x, parentPosition.y + 0.4f, 0);
                        leftHalfFruit.SetActive(true);
                    }
                    break;
                }
            case "Fruit3":
                {
                    gameObjectTag = "HalfLemon";
                    leftHalfFruit = GetPooledObject(gameObjectTag);
                    if (leftHalfFruit != null)
                    {
                        leftHalfFruit.GetComponent<FruitHalf>().fallRight = false;
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;   // Converting fron Radius to Degrees
                        leftHalfFruit.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                        leftHalfFruit.transform.localScale = new Vector3(1f, -1f, 1f);
                        if (angle >= 0 && angle < 90) leftHalfFruit.transform.position = new Vector3(parentPosition.x, parentPosition.y + 0.7f, 0);
                        else if (angle >= 90 && angle <= 180) leftHalfFruit.transform.position = new Vector3(parentPosition.x, parentPosition.y, 0);
                        else leftHalfFruit.transform.position = new Vector3(parentPosition.x, parentPosition.y, 0);
                        leftHalfFruit.SetActive(true);
                    }
                    break;
                }
        }
    }

    #endregion Methods
}
// EOF - End Of File