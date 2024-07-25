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

    [SerializeField] private GameObject bulletPrefab;

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
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(bulletPrefab, this.transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }

        fireRate = bulletFireRate;
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
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
