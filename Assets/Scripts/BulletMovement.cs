using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BulletMovement : MonoBehaviour
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
        StartCoroutine(DestroyBullet());
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
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
        if (collision != null)
        {
            gameObject.SetActive(false);
        }
    }
}
