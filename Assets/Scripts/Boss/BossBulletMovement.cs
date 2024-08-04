using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossBulletMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    private float direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float movementSpeed;

        //StartCoroutine(DestroyBullet());
        movementSpeed = speed * Time.deltaTime * direction;

        transform.Translate(movementSpeed, 0, 0, Space.World);
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
        if (collision != null && collision.gameObject.CompareTag("BossWall"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().Death();
        }
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
