using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBoss : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public GameObject wallBoss1;
    public GameObject wallBoss2;

    public bool isActivated;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            player1.transform.position = collision.transform.position;
            player2.transform.position = collision.transform.position;
            isActivated = true;
            wallBoss1.SetActive(true);
            wallBoss2.SetActive(true);
            print("Activated");
        }
    }
}
