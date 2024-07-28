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

    private bool isRocket;
    private bool isBullet;
    private bool isBulletOscilation;

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
        isBullet = true;
        ObjectToPool(1);
    }

    public void ObjectToPool(int WeaponID)
    {
        isBullet = false;
        isBulletOscilation = false;
        isRocket = false;

        switch (WeaponID)
        {
            case 0:
                if (pooledObjects.Count > 0)
                {
                    pooledObjects.Clear();
                }
                break;

            case 1:
                if (pooledObjects.Count > 0)
                {
                    pooledObjects.Clear();
                }
                fireRate = bulletFireRate;
                isBullet = true;
                for (int i = 0; i < amountToPool; i++)
                {
                    GameObject obj = Instantiate(bulletPrefab, this.transform);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                };
                break;

            case 2:
                if (pooledObjects.Count > 0)
                {
                    pooledObjects.Clear();
                }
                fireRate = bulletFireRate;
                isBulletOscilation = true;
                for (int i = 0; i < amountToPool; i++)
                {
                    GameObject obj = Instantiate(bulletOscilationPrefab, this.transform);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                };
                break;

            case 3:
                if (pooledObjects.Count > 0)
                {
                    pooledObjects.Clear();
                }
                fireRate = rocketFireRate;
                isRocket = true;
                for (int i = 0; i < amountToPool; i++)
                {
                    GameObject obj = Instantiate(rocketPrefab, this.transform);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                };
                break;
        }
    }

    public GameObject GetPooledObject()
    {
        if (pooledObjects != null)
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    pooledObjects[i].gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    return pooledObjects[i];
                }
            }
        }
        return null;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public bool IsBullet()
    {
        return isBullet;
    }

    public bool IsBulletOsci()
    {
        return isBulletOscilation;
    }

    public bool IsRocket()
    {
        return isRocket;
    }
}
