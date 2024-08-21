using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class Playah : MonoBehaviour
{


    //Movement Variables
    [SerializeField, Range(1, 20)] private float speed = 5;
    [SerializeField, Range(1, 20)] private float jumpForce = 10;
    [SerializeField, Range(0.01f, 1)] private float groundCheckRadius = 0.02f;
    [SerializeField] private LayerMask isGroundLayer;

    //GroundCheck Stuff
    private Transform groundCheck;
    private bool isGrounded = false;
    [SerializeField] private int maxLives = 10;
    //Component References
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //Component References Filled
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        //Checking values to ensure non-garbage data
        if (speed <= 0)
        {
            speed = 5;

        }

        if (jumpForce <= 0)
        {
            jumpForce = 10;

        }

        //Creating groundcheck object
        if (!groundCheck)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "GroundCheck";
            groundCheck = obj.transform;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Grab horizontal axis - Check Project Settings > Input Manager to see the inputs defined
        float hInput = Input.GetAxis("Horizontal");

        //AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        //Creat a small overlap collider to check if we are touching the ground
        IsGrounded();

        //if (curPlayingClips.Length > 0)
        //  {
        //if (curPlayingClips[0].clip.name == "AsadAttack")
        //rb.velocity = new Vector2(0, rb.velocity.y);
        // else
        // {
        //rb.velocity = new Vector2(hInput * speed, rb.velocity.y);
        // }
        //  }

        rb.velocity = new Vector2(hInput * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown("Fire1") && isGrounded)
        {
            anim.SetTrigger("AsadAttack");
        }

        if (Input.GetButtonDown("Fire1") && !isGrounded)
        {
            anim.SetTrigger("isJumpAttacking");
        }


        //Button Input Checks
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //Sprite Flipping
        if (hInput != 0) sr.flipX = (hInput < 0);

        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded);
    }
    void IsGrounded()
    {
        if (!isGrounded)
        {
            if (rb.velocity.y <= 0)
            {
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
            }
        }
        else
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
    }

    void IncreaseGravity()
    {
        rb.gravityScale = 10;
    }

    private Coroutine jumpForceChange;
    private Coroutine speedChange;

    public void PowerupValueChange(Pickup.PickupType type)
    {
        if (type == Pickup.PickupType.PowerupSpeed)
            StartPowerupCoroutine(ref speedChange, ref speed, type);

        if (type == Pickup.PickupType.PowerupJump)
            StartPowerupCoroutine(ref jumpForceChange, ref jumpForce, type);

    }

    public void StartPowerupCoroutine(ref Coroutine InCoroutine, ref float inVar, Pickup.PickupType type)
    {
        if (jumpForceChange != null)
        {
            StopCoroutine(InCoroutine);
            InCoroutine = null;
            inVar /= 2;

        }

        InCoroutine = StartCoroutine(PowerupChange(type));
    }

    IEnumerator PowerupChange(Pickup.PickupType type)
    {
        //this code runs before the wait
        if (type == Pickup.PickupType.PowerupSpeed)
            speed *= 2;

        if (type == Pickup.PickupType.PowerupJump)
            jumpForce *= 2;


        Debug.Log($"Jump force value is {jumpForce}, Speed Value is {speed}");

        yield return new WaitForSeconds(5.0f);

        //this code runs after the wait
        if (type == Pickup.PickupType.PowerupSpeed)
        {
            speed /= 2;
            speedChange = null;
        }
        if (type == Pickup.PickupType.PowerupJump)
        {
            jumpForce /= 2;
            jumpForceChange = null;
        }


        Debug.Log($"Jump force value is {jumpForce}");


    }

    //Private Lives Variable
    private int _lives = 5;

    //public variable for getting and setting lives

    public int lives
    {
        get
        {
            return _lives;
        }
        set
        {
            //all lives lost (zero counts as a life due to the check)
            if (value < 0)
            {
                //game over function called here
                //return to prevent the rest of the function to be called
                return;
            }

            //lost a life
            if (value < _lives)
            {
                //Respawn function called he re

            }

            if (value > maxLives)
            {
                value = maxLives;
            }

            _lives = value;

            Debug.Log($"Lives value on {gameObject.name} has changed to {lives}");
        }
    }
}
