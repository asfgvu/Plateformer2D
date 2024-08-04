using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    private bool canShoot = true;
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject bossBullet;
    [SerializeField] private Transform firePoint;

    [SerializeField] private GameObject boss;
    private Boss bossScript;

    // Start is called before the first frame update
    void Start()
    {
        bossScript = boss.GetComponent<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
        {
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        GameObject instantiateObj =  Instantiate(bossBullet, firePoint);
        instantiateObj.GetComponent<BossBulletMovement>().SetDirection(Mathf.Sign(transform.localScale.x));

        canShoot = false;

        StartCoroutine(FireRateHandler());

        yield return null;
    }

    private IEnumerator FireRateHandler()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletMovement bulletMovement = collision.gameObject.GetComponent<BulletMovement>();

            if (bulletMovement != null)
            {
                print("BulletMovement not null");
                float damage = bulletMovement.GetDamage();
                bossScript.TakeDamage(damage);
            }
        }
    }
}
