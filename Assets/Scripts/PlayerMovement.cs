using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    private bool doubleJump;

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
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    private float wallClimbingSpeed = 8f;
    private float wallClimbDuration = 1f;
    private float currentWallClimbDuration;
    private float vertical;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform firePoint;

    [SerializeField] private LayerMask grapLayer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private DistanceJoint2D distanceJoint;
    [SerializeField] private float grappleDetectionDistance;
    private Collider2D grappleTouched;

    private void Start()
    {
        currentWallClimbDuration = wallClimbDuration;

        distanceJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        dash = Input.GetButtonDown("Dash");

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

                doubleJump = !doubleJump;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (dash && canDash)
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        WallJump();
        WallClimb();

        if (!isWallJumping)
        {
            Flip();
        }

        if (IsGrounded())
        {
            currentWallClimbDuration = wallClimbDuration;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

        Collider[] hitCollider = Physics.OverlapSphere(gameObject.transform.position, grappleDetectionDistance);

        if (hitCollider != null)
        {
            for (int i = 0; i < hitCollider.Length; i++)
            {
                print(hitCollider[i]);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector2 grapplePos = (Vector2)hitCollider[0].gameObject.transform.position;
                lineRenderer.SetPosition(0, grapplePos);
                lineRenderer.SetPosition(1, transform.position);
                distanceJoint.connectedAnchor = grapplePos;
                distanceJoint.enabled = true;
                lineRenderer.enabled = true;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                distanceJoint.enabled = false;
                lineRenderer.enabled = false;
            }

            if (distanceJoint.enabled)
            {
                lineRenderer.SetPosition(1, transform.position);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool isWalled()
    {
        var collidersInRangeGround = Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);

        if (collidersInRangeGround != null)
        {
            if (collidersInRangeGround.gameObject.tag == "Wall")
            {
                return Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
            }
        }
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
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
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
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

    private void Fire()
    {
        GameObject bullet = ObjectPool.instance.GetPooledObject();

        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.GetComponent<BulletMovement>().SetDirection(Mathf.Sign(transform.localScale.x));
            bullet.SetActive(true);
        }
    }
}
