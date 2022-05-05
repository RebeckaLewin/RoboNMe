using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A script attached to the boxes in the scene
//Can be pushed and pulled, but are blocked by Robo
public class BoxScript : MonoBehaviour
{
    private GameObject player;
    private float distanceToPlayer;
    public bool isBeingMoved { get; set; }
    private float hInput;
    public bool blocked { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingMoved && !blocked)
        {
            Vector2 newPos = new Vector2(player.transform.position.x + distanceToPlayer, transform.position.y);
            transform.position = newPos;
        }

        if (blocked && Input.GetAxisRaw("Horizontal") == -hInput)
            blocked = false;
    }

    public void SetMoveStatus(bool isMoved)
    {
        if (isMoved)
        {
            distanceToPlayer = transform.position.x - player.transform.position.x;
            isBeingMoved = true;
        }

        else
        {
            isBeingMoved = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hInput = Input.GetAxisRaw("Horizontal");
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(hInput, 0), 0.5f);
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("robo"))
                {
                    blocked = true;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("robo"))
        {
            blocked = true;
        }
    }
}
