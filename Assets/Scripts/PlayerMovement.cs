using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 10f;
    private float grapplingSpeed = .1f;
    private float jumpingPower = 16.25f;
    private bool isFacingRight = true;

    private bool doubleJump;
    private bool canDoubleJump;

    private bool canDash = true;
    private bool isDashing;
    private float horizontalDashingPower = 30f;
    private float verticalDashingPower = 15f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private bool dash;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16.25f);

    private float wallClimbingSpeed = 8f;
    [SerializeField] private float wallClimbDuration = 1f;
    private float currentWallClimbDuration;
    private float vertical;
    private bool canWallJump = true;

    [SerializeField] private int playerNumber;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private GameObject Bullet;
    public Transform firePoint;

    [SerializeField] private LayerMask grapLayer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private DistanceJoint2D distanceJoint;
    [SerializeField] private float grappleDetectionDistance;
    [SerializeField] private Color grappleFocusColor;
    [SerializeField] private Color defaultGrappleColor;
    public Rigidbody2D plateformRb;
    public bool isOnPlateform;
    private Collider2D grappleTouched;
    private bool isGrappling = false;
    private bool canAddInputGrappling;
    private Collider grappleFocus;
    private bool canShoot = true;
    private float fireRate;
    private GameObject bullet;

    private List<GameObject> CrystalList = new List<GameObject>();

    private void Start()
    {
        currentWallClimbDuration = wallClimbDuration;

        distanceJoint.enabled = false;

        //string[] names = Input.GetJoystickNames();
        //Debug.Log("Connected Joysticks:");
        //for (int i = 0; i < names.Length; i++)
        //{
        //    Debug.Log("Joystick" + (i + 1) + " = " + names[i]);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal" + playerNumber);
        vertical = Input.GetAxisRaw("Vertical" + playerNumber);
        dash = Input.GetButtonDown("Dash" + playerNumber);

        if (IsGrounded() && !Input.GetButton("Jump" + playerNumber))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump" + playerNumber))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

                doubleJump = !doubleJump;
                canDoubleJump = !canDoubleJump;
            } 
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                canDoubleJump = !canDoubleJump;
            }
        }

        if (Input.GetButtonUp("Jump" + playerNumber) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (dash && canDash && !isGrappling)
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        WallJump();
        WallClimb();
        Grappling();

        if (!isWallJumping)
        {
            Flip();
        }

        if (IsGrounded())
        {
            currentWallClimbDuration = wallClimbDuration;
            canWallJump = true;
            canDoubleJump = true;
        }

        float fireAxis = Input.GetAxisRaw("Fire" + playerNumber);

        if (fireAxis != 0)
        {
            if (canShoot)
            {
                StartCoroutine(Fire());
            }
            
        }

        if (Input.GetButtonDown("Respawn"))
        {
            print("Respawn");
            transform.position = Respawn().transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (isOnPlateform)
        {
            rb.velocity = new Vector2(horizontal * speed + plateformRb.velocity.x, rb.velocity.y);
        }
        else if (!isGrappling)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        if (isGrappling && canAddInputGrappling)
        {
            rb.velocity = new Vector2(rb.velocity.x + horizontal * grapplingSpeed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
    }

    private bool isWalled()
    {
        var collidersInRangeGround = Physics2D.OverlapCircle(wallCheck.position, 0.4f, groundLayer);

        if (collidersInRangeGround != null)
        {
            if (collidersInRangeGround.gameObject.tag == "Wall")
            {
                return collidersInRangeGround;
            }
        }
        return Physics2D.OverlapCircle(wallCheck.position, 0.4f, wallLayer);
    }

    private void Grappling() 
    {
        Collider[] hitCollider = Physics.OverlapSphere(gameObject.transform.position, grappleDetectionDistance, grapLayer);

        //print(hitCollider.Length);

        if (hitCollider.Length > 0)
        {
            int ColliderArrayNumber = 0;
            float lowerDistance = 100;
            float currentDistance;

            grappleFocus = hitCollider[0];

            if (hitCollider.Length > 1)
            {
                for (int i = 0; i < hitCollider.Length; i++)
                {
                    currentDistance = Vector3.Distance(transform.position, hitCollider[i].gameObject.transform.position);

                    if (currentDistance < lowerDistance)
                    {
                        lowerDistance = currentDistance;
                        ColliderArrayNumber = i;
                        grappleFocus = hitCollider[i];
                    }
                }
            }

            for (int i = 0; i < hitCollider.Length; i++)
            {
                if (grappleFocus == hitCollider[i])
                {
                    hitCollider[i].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
                else
                {
                    hitCollider[i].gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }

            if (Input.GetButtonDown("Grappling" + playerNumber))
            {
                Vector2 grapplePos = (Vector2)hitCollider[ColliderArrayNumber].gameObject.transform.position;
                lineRenderer.SetPosition(0, grapplePos);
                lineRenderer.SetPosition(1, transform.position);
                distanceJoint.connectedAnchor = grapplePos;
                distanceJoint.enabled = true;
                lineRenderer.enabled = true;
                isGrappling = true;
                //canDash = true;
                //doubleJump = true;
                rb.gravityScale = 4f;

                if (rb.velocity.x >= 10 && rb.velocity.y >= 10)
                {
                    canAddInputGrappling = false;
                }
                else
                {
                    canAddInputGrappling = true;
                }
            }
            else if (Input.GetButtonUp("Grappling" + playerNumber))
            {
                distanceJoint.enabled = false;
                lineRenderer.enabled = false;
                isGrappling = false;
                rb.gravityScale = 4f;
                canAddInputGrappling = true;
            }

            if (distanceJoint.enabled)
            {
                lineRenderer.SetPosition(1, transform.position);
            }
        }
        else
        {
            //grappleFocus.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void WallSlide()
    {
        if (isWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallClimb()
    {
        if (isWalled() && !IsGrounded() && vertical > 0)
        {
            if (currentWallClimbDuration > 0)
            {
                isWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallClimbingSpeed, float.MaxValue));
                currentWallClimbDuration -= Time.deltaTime;
            }
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding && canWallJump)
        {
            isWallJumping = false;
            canWallJump = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump" + playerNumber) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * horizontalDashingPower, transform.localScale.y * vertical * verticalDashingPower);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private IEnumerator Fire()
    {
        if (playerNumber == 1)
        {
            bullet = ObjectPool.instance.GetPooledObject1();
        }

        if (playerNumber == 2)
        {
            bullet = ObjectPool.instance.GetPooledObject2();
        }

        canShoot = false;

        if (bullet != null)
        {
            if (playerNumber == 1)
            {
                print("Bullet not null1");
            }

            if (playerNumber == 2)
            {
                print("Bullet not null 2");
            }
            
            bullet.transform.position = firePoint.position;
            bullet.GetComponent<BulletMovement>().SetDirection(Mathf.Sign(transform.localScale.x));
            bullet.SetActive(true);
        }
        else
        {
            if (playerNumber == 1)
            {
                print("Bullet null1");
            }

            if (playerNumber == 2)
            {
                print("Bullet null 2");
            }
        }

        StartCoroutine(FireRateHandler());

        yield return null;
    }

    private IEnumerator FireRateHandler()
    {
        if (playerNumber == 1)
        {
            fireRate = ObjectPool.instance.GetFireRate1();
        }

        if (playerNumber == 2)
        {
            fireRate = ObjectPool.instance.GetFireRate2();
        }
        
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    public GameObject Respawn()
    {
        return CheckpointManager.instance.GetLastCheckpoint();
    }

    public void Death()
    {
        transform.position = Respawn().transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trap")
        {
            for (int i = 0; i < CrystalList.Count; i++)
            {
                CrystalList[i].gameObject.SetActive(true);
            }
            CrystalList.Clear();
            Death();
        }

        if (collision.tag == "ResetDash")
        {
            canDash = true;
            collision.gameObject.SetActive(false);
            CrystalList.Add(collision.gameObject);
        }
    }
}
