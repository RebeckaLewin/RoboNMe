using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the script that contains methods to make Robo do things, as well as play his animation when needed
public class RoboScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpLerp;
    public Vector2 Direction { get; set; }
    private bool move, jump;
    private RoboManager RM;
    private Animator animator;
    private float currentJump;
    private GameObject pt;

    [Header("SOUNDS")]
    [SerializeField] private AudioClip turn, clank;
    private AudioSource audioSource;

    private void Start()
    {
        RM = GetComponent<RoboManager>();
        Direction = Vector2.right;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (move && !animator.GetCurrentAnimatorStateInfo(0).IsName("robo_anticipation"))
        {
            transform.Translate(Direction * moveSpeed * Time.deltaTime);
            animator.Play("robo_walk");
        }

        if (jump)
        {
            currentJump = Mathf.Lerp(currentJump, jumpHeight, jumpLerp);
            transform.Translate(Vector2.up * currentJump);
        }

    }

    public void Jump()
    {
        currentJump = 0;
        animator.Play("robo_jump");
        StartCoroutine(AnticipateJump());
    }

    public bool Move()
    {
        bool blocked = false;
        Vector2 lowerPos = new Vector2(transform.position.x, transform.position.y - 0.2f);

        RaycastHit2D[] hits = Physics2D.RaycastAll(lowerPos, Direction, 1f);
        if(hits.Length > 0)
        {
            foreach(RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("wall") || hit.collider.gameObject.CompareTag("box"))
                {
                    blocked = true;
                    return blocked;
                }
            }
        }
        move = true;
        return blocked;
    }

    public void Turn()
    {
        Vector2 scale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        Direction *= -1;
        transform.localScale = scale;
        audioSource.clip = turn;
        audioSource.Play();
    }

    public void Delay()
    {
        animator.Play("robo_idle");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("wall") || collision.gameObject.CompareTag("box")) && move)
        {
            move = false;
            animator.Play("robo_idle");
            RM.SendCommand();
        }

        else if(collision.gameObject.CompareTag("floor") && jump)
        {
            jump = false;
            animator.Play("robo_anticipation");
            RM.SendCommand();
        }

        else if(collision.gameObject.CompareTag("passthrough") && !move)
        {
            pt = collision.gameObject;
            float dir = Mathf.Sign(transform.position.y - pt.transform.position.y);
            if(dir < 0)
            {
                Physics2D.IgnoreCollision(pt.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
                StartCoroutine(BringBackCollision());
            }
            else
            {
                jump = false;
                animator.Play("robo_anticipation");
                RM.SendCommand();
            }
        }
    }

    private IEnumerator AnticipateJump()
    {
        yield return new WaitForSeconds(0.2f);
        jump = true;
    }

    public void PlayWalk()
    {
        audioSource.clip = clank;
        audioSource.Play();
    }

    private IEnumerator BringBackCollision()
    {
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(pt.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), false);
        pt = null;
    }
}
