using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float rocketSpeed = 2f;
    [SerializeField] private float bulletDamage = 10;
    [SerializeField] private float rocketDamage = 50;
    private float direction;

    [SerializeField] private bool isRocket;
    [SerializeField] private bool isOscilation;

    private float damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float movementSpeed;

        StartCoroutine(DestroyBullet());
        if (ObjectPool.instance.IsRocket() || ObjectPool.instance.IsRocket2())
        {
            movementSpeed = rocketSpeed * Time.deltaTime * direction;
            damage = rocketDamage;
        }
        else
        {
            movementSpeed = speed * Time.deltaTime * direction;
            damage = bulletDamage;
        }
        
        if (ObjectPool.instance.IsBulletOsci() || ObjectPool.instance.IsBulletOsci2())
        {
            Vector2 pos = transform.position;
            pos.x += movementSpeed;

            float sin = Mathf.Sin(pos.x) * 0.1f;
            pos.y = sin * 0.1f;

            transform.Translate(movementSpeed, pos.y, 0);

            damage = bulletDamage;
        }
        else
        {
            transform.Translate(movementSpeed, 0, 0);
            damage = bulletDamage;
        }
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;

        float localScaleX = transform.localScale.x;

        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && !collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            gameObject.SetActive(false);
        }
    }

    public float GetDamage()
    {
        return damage;
    }
}
