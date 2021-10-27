using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Rigidbody2D rg;
    [SerializeField] float movementSpeed;
    [SerializeField] float maxSpeed;
    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float raydis;
    [SerializeField] LayerMask GroundLayerMask;
    [SerializeField] float jumpRequestBufferDuration = 0.2f;
    float lastJumpRequestTime = Mathf.NegativeInfinity;
    [Header("Dash")]
    [SerializeField] float dashForce;
    [SerializeField] float timeBetweenDashes;
    bool dash = true;



    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Time.timeSinceLevelLoad < lastJumpRequestTime + jumpRequestBufferDuration)
            Jump();

        // Dash
        if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift) && dash)
        {
            rg.velocity = new Vector2(0, rg.velocity.y);
            rg.AddForce(new Vector2(-dashForce, 0f), ForceMode2D.Impulse);
            StartCoroutine("DashWaitTime");
        }
        if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift) && dash)
        {
            rg.velocity = new Vector2(0, rg.velocity.y);
            rg.AddForce(new Vector2(dashForce, 0f), ForceMode2D.Impulse);
            StartCoroutine("DashWaitTime");
        }
    }

    private void FixedUpdate()
    {
        //Movements
        float moveX = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.A) && rg.velocity.x > -maxSpeed)
            rg.AddForce(new Vector2(moveX * movementSpeed, 0f), ForceMode2D.Force);
        if (Input.GetKey(KeyCode.D) && rg.velocity.x < maxSpeed)
            rg.AddForce(new Vector2(moveX * movementSpeed, 0f), ForceMode2D.Force);
    }

    IEnumerator DashWaitTime()
    {
        dash = false;
        yield return new WaitForSeconds(timeBetweenDashes);
        dash = true;
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raydis, GroundLayerMask);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Ground")
            {
                return true;
            }
            else return false;
        }
        else
        {
            return false;
        }

    }

    void Jump()
    {

        if (IsGrounded())
        {
            rg.velocity = new Vector2(rg.velocity.x, 0f);
            rg.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            lastJumpRequestTime = Mathf.NegativeInfinity;
        }
        else
        {
            lastJumpRequestTime = Time.timeSinceLevelLoad;
        }
    }

}