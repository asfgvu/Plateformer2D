using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WallBoss : MonoBehaviour
{
    public ActivateBoss ActivateBoss;
    public float speed;
    public CameraShake CameraShake;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime; // calculate distance to move
        if (ActivateBoss.isActivated)
        {
            if (transform.position.y <= 15f)
            {
                transform.position += Vector3.up * Time.deltaTime * speed;
                StartCoroutine(CameraShake.Shake(2f, .1f));
            }
        }
    }
}
