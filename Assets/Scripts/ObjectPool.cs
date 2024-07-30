using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private List<GameObject> pooledObjects1 = new List<GameObject>();
    private List<GameObject> pooledObjects2 = new List<GameObject>();
    private int amountToPool = 20;

    private float fireRate1;
    private float fireRate2;
    private float bulletFireRate = 0.25f;
    private float rocketFireRate = 1f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private GameObject bulletOscilationPrefab;
    [SerializeField] private Transform pool1;
    [SerializeField] private Transform pool2;

    private bool isRocket;
    private bool isBullet;
    private bool isBulletOscilation;
    private bool isRocket2;
    private bool isBullet2;
    private bool isBulletOscilation2;

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
        ObjectToPool1(0);
        ObjectToPool2(0);
    }

    public void ObjectToPool1(int WeaponID)
    {
        foreach (GameObject obj in  pooledObjects1)
        {
            Destroy(obj);
        }
        pooledObjects1.Clear();
        isBullet = false;
        isBulletOscilation = false;
        isRocket = false;

        switch (WeaponID)
        {
            case 0:
                break;

            case 1:
                fireRate1 = bulletFireRate;
                isBullet = true;
                for (int i = 0; i < amountToPool; i++)
                {
                    GameObject obj = Instantiate(bulletPrefab, this.transform);
                    obj.SetActive(false);
                    pooledObjects1.Add(obj);
                    obj.transform.SetParent(pool1);
                };
                break;

            case 2:
                fireRate1 = bulletFireRate;
                isBulletOscilation = true;
                for (int i = 0; i < amountToPool; i++)
                {
                    GameObject obj = Instantiate(bulletOscilationPrefab, this.transform);
                    obj.SetActive(false);
                    pooledObjects1.Add(obj);
                    obj.transform.SetParent(pool1);
                };
                break;

            case 3:
                fireRate1 = rocketFireRate;
                isRocket = true;
                for (int i = 0; i < amountToPool; i++)
                {
                    GameObject obj = Instantiate(rocketPrefab, this.transform);
                    obj.SetActive(false);
                    pooledObjects1.Add(obj);
                    obj.transform.SetParent(pool1);
                };
                break;
        }
    }

    public void ObjectToPool2(int WeaponID)
    {
        foreach (GameObject obj in pooledObjects2)
        {
            Destroy(obj);
        }
        pooledObjects2.Clear();
        isBullet2 = false;
        isBulletOscilation2 = false;
        isRocket2 = false;

        switch (WeaponID)
        {
            case 0:
                break;

            case 1:
                fireRate2 = bulletFireRate;
                isBullet2 = true;
                for (int i = 0; i < amountToPool; i++)
                {
                    GameObject obj = Instantiate(bulletPrefab, this.transform);
                    obj.SetActive(false);
                    pooledObjects2.Add(obj);
                    obj.transform.SetParent(pool2);
                };
                break;

            case 2:
                fireRate2 = bulletFireRate;
                isBulletOscilation2 = true;
                for (int i = 0; i < amountToPool; i++)
                {
                    GameObject obj = Instantiate(bulletOscilationPrefab, this.transform);
                    obj.SetActive(false);
                    pooledObjects2.Add(obj);
                    obj.transform.SetParent(pool2);
                };
                break;

            case 3:
                fireRate2 = rocketFireRate;
                isRocket2 = true;
                for (int i = 0; i < amountToPool; i++)
                {
                    GameObject obj = Instantiate(rocketPrefab, this.transform);
                    obj.SetActive(false);
                    pooledObjects2.Add(obj);
                    obj.transform.SetParent(pool2);
                };
                break;
        }
    }

    public GameObject GetPooledObject1()
    {
        if (pooledObjects1 != null)
        {
            for (int i = 0; i < pooledObjects1.Count; i++)
            {
                if (!pooledObjects1[i].activeInHierarchy)
                {
                    pooledObjects1[i].gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    return pooledObjects1[i];
                }
            }
        }
        return null;
    }

    public GameObject GetPooledObject2()
    {
        if (pooledObjects2 != null)
        {
            for (int i = 0; i < pooledObjects2.Count; i++)
            {
                if (!pooledObjects2[i].activeInHierarchy)
                {
                    pooledObjects2[i].gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    return pooledObjects2[i];
                }
            }
        }
        return null;
    }

    public float GetFireRate1()
    {
        return fireRate1;
    }

    public float GetFireRate2()
    {
        return fireRate2;
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

    public bool IsBullet2()
    {
        return isBullet2;
    }

    public bool IsBulletOsci2()
    {
        return isBulletOscilation2;
    }

    public bool IsRocket2()
    {
        return isRocket2;
    }
}
