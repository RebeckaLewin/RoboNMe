using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles user input from keyboard, and controls Mimi, as well as animates her
public class PlayerController : MonoBehaviour
{
    [Header("MOVEMENT")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float pushSpeed;
    private float speed;
    [SerializeField] private float dragTime;
    private float hInput;
    private float inputDelay;
    private float xVelocity = 0;

    [Header("JUMP")]
    [SerializeField] private float jumpHeight;
    private bool jumping, hasLanded;
    private GameObject currentPlatform;

    //variables for pushing and pulling boxes
    private bool pushingOrPulling;
    private bool nextToBox;
    private BoxScript box;

    [Header("ROBO")]
    [SerializeField] private GameObject robo;
    private BoxCollider2D roboBox;
    private bool onRobo;
    private bool isSitting; //mechanic that didn't make it in time

    //animation variables
    private Vector2 startScale, tempScale;
    private bool facingRight = true, anticipating;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        roboBox = robo.GetComponent<BoxCollider2D>();
        startScale = transform.localScale;
        tempScale = startScale;
    }

    private void Update()
    {
        if (GameManager.State != GameStates.playPhase)
            return;

        if (!pushingOrPulling)
        {
            if (speed != maxSpeed)
                speed = maxSpeed;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (isSitting)
                    isSitting = false;
                else if (!jumping)
                    Jump();
            }


            if (Input.GetKeyDown(KeyCode.DownArrow) && onRobo)
            {
                isSitting = true;
            }

            if (facingRight)
            {
                tempScale.x = startScale.x;
                transform.localScale = tempScale;
            }
            else
            {
                tempScale.x = -startScale.x;
                transform.localScale = tempScale;
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                box.SetMoveStatus(false);
                pushingOrPulling = false;
            }

        }

        //starts to push and pull the box
        if (Input.GetKeyDown(KeyCode.Space) && nextToBox && !pushingOrPulling)
        {
            float distance = Mathf.Sign(transform.position.x - box.gameObject.transform.position.x);
            Vector2 newPos = new Vector2(box.gameObject.transform.position.x + (0.5f * distance), transform.position.y);
            if (distance < 0)
            {
                facingRight = true;
                tempScale.x = startScale.x;
                transform.localScale = tempScale;
            }


            else
            {
                facingRight = false;
                tempScale.x = -startScale.x;
                transform.localScale = tempScale;
            }

            transform.position = newPos;
            box.SetMoveStatus(true);
            speed = pushSpeed;
            pushingOrPulling = true;
        }
    }

    private void FixedUpdate()
    {
        if(GameManager.State == GameStates.playPhase)
        {
            if (!isSitting)
            {
                Move();
            }
            else { Sit(); }
        }
    }

    //the method for moving Mimi around
    private void Move()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        inputDelay = Mathf.SmoothDamp(inputDelay, hInput, ref xVelocity, dragTime);


        if (inputDelay < 0.2f && inputDelay > -0.2f && !jumping)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("mimi_land") && hInput == 0 && !pushingOrPulling)
                animator.Play("mimi_idle");
        }

        else if (inputDelay != 0)
        {
            if (box == null || !box.blocked || !pushingOrPulling)
                transform.Translate(Vector2.right * inputDelay * speed * Time.deltaTime);
            if(!jumping && !pushingOrPulling)
                animator.Play("mimi_run");
        }

        if (pushingOrPulling && box != null)
        {
            float xDir =  Mathf.Sign(box.transform.position.x - transform.position.x);
            if (hInput != xDir)
                animator.Play("mimi_pull");
            else if (hInput == xDir)
                animator.Play("mimi_push");
        }
        else
        {
            if (hInput > 0)
                facingRight = true;
            else if (hInput < 0)
                facingRight = false;
        }
    }

    //method for jumping
    private void Jump()
    {
        //checks so that Mimi is not blocked
        bool blocked = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.up, 1f);
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("floor"))
            {
                blocked = true;
                return;
            }
        }
        if (!blocked)
        {
            jumping = true;
            animator.Play("mimi_jump");
            anticipating = true;
            StartCoroutine(JumpDelay());
        }
    }

    //delays the jump so that the anticipation animation can finish playing
    private IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        anticipating = false;
    }

    private void Sit()
    {
        float yPos = ((GetComponent<CapsuleCollider2D>().size.y * transform.localScale.y) / 2) + ((roboBox.size.y * robo.transform.localScale.y) / 2);
        transform.position = new Vector2(robo.transform.position.x, robo.transform.position.y + yPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("robo"))
        {
            onRobo = true;
            jumping = false;
            Physics2D.IgnoreCollision(robo.GetComponent<BoxCollider2D>(), GetComponent<CapsuleCollider2D>(), false);
        }

        if (collision.gameObject.CompareTag("platform"))
        {
            if (collision.gameObject.GetComponentInParent<BoxScript>() != null && jumping && !nextToBox)
            {
                Physics2D.IgnoreCollision(collision.gameObject.transform.parent.GetComponent<BoxCollider2D>(), GetComponent<CapsuleCollider2D>(), false);
                jumping = false;
            }
            currentPlatform = collision.gameObject.transform.parent.gameObject;
        }


        else if (collision.gameObject.CompareTag("box"))
        {
            nextToBox = true;
            box = collision.gameObject.GetComponent<BoxScript>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("robo"))
        {
            onRobo = false;
        }

        else if (collision.gameObject.CompareTag("box"))
        {
            nextToBox = false;
            box.SetMoveStatus(false);
            pushingOrPulling = false;
            box = null;
        }

        else if (collision.gameObject.CompareTag("platform"))
        {
            currentPlatform = null;
            hasLanded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("robo"))
        {
            if (!onRobo)
            {
                Physics2D.IgnoreCollision(roboBox, GetComponent<CapsuleCollider2D>());
            }
            else { jumping = false; animator.Play("mimi_land"); }
        }

        else if (collision.gameObject.CompareTag("box") && nextToBox)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), GetComponent<CapsuleCollider2D>());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!hasLanded)
        {
            if ((collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("passthrough")) && !anticipating && !pushingOrPulling && collision.gameObject == currentPlatform)
            {
                jumping = false;
                animator.Play("mimi_land");
                hasLanded = true;
            }
        }
    }
}
