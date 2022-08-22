using System.Collections.Generic;
using UnityEngine;

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

    // Particle Effect Attributes
    [SerializeField] private Material[] fruitMaterials;
    [SerializeField] private Material skullMaterial;
    [SerializeField] private string[] fruitTags;

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

    public void SpawnParticleEffect(GameObject spawnerObject)
    {
        Sound_Controller.SharedInstance.PlayFruitSound();

        GameObject gameObject = Particle_Spawner.SharedInstance.GetPooledObject();
        if (gameObject != null)
        {
            if (spawnerObject.tag != "Skull")   // Fruit
            {
                for (int i = 0; i < fruitMaterials.Length; i++)
                    if (spawnerObject.tag == fruitTags[i])
                    {
                        gameObject.GetComponent<Renderer>().material = fruitMaterials[i];
                    }
            }
            else gameObject.GetComponent<Renderer>().material = skullMaterial;

            gameObject.transform.position = spawnerObject.gameObject.transform.position;
            gameObject.SetActive(true);
        }
        
    }

    #endregion Object Pooling Method

    #endregion Methods
}
   // EOF - End Of File