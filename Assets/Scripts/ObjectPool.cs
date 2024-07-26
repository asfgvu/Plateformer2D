using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 20;

    private float fireRate;
    private float bulletFireRate = 0.25f;
    private float rocketFireRate = 1f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private GameObject bulletOscilationPrefab;

    private GameObject instantiateObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instantiateObject = bulletOscilationPrefab;

        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(instantiateObject, this.transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }

        if (instantiateObject == rocketPrefab)
        {
            fireRate = rocketFireRate;
        }

        if (instantiateObject == bulletPrefab || instantiateObject == bulletOscilationPrefab)
        {
            fireRate = bulletFireRate;
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                return pooledObjects[i];
            }
        }

        return null;
    }

    public float GetFireRate()
    {
        return fireRate;
    }


}
