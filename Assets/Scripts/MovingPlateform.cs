using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int startingPoint;
    [SerializeField] private Transform[] points;

    PlayerMovement playerMovement1;
    PlayerMovement playerMovement2;
    Rigidbody2D rb;
    Vector3 moveDirection;

    private int i;

    private void Awake()
    {
        playerMovement1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerMovement>();
        //playerMovement2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;

        DirectionCalculate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
            DirectionCalculate();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }

    void DirectionCalculate()
    {
        moveDirection = (points[i].position - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            playerMovement1.isOnPlateform = true;
            playerMovement1.plateformRb = rb;
        }

        if (collision.CompareTag("Player2"))
        {
            playerMovement2.isOnPlateform = true;
            playerMovement2.plateformRb = rb;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            playerMovement1.isOnPlateform = false;
        }

        if (collision.CompareTag("Player2"))
        {
            playerMovement2.isOnPlateform = false;
        }
    }
}
