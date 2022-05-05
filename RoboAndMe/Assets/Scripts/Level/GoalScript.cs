using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attaches to the goal (star), ends game if both Mimi and Robo are touching it
public class GoalScript : MonoBehaviour
{
    private int numOfKids;
    [SerializeField] private GameObject goalUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("robo"))
        {
            numOfKids++;
        }

        if(numOfKids == 2)
        {
            LeanTween.moveLocalY(goalUI, 0, 2).setEaseOutBounce();
            GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("robo"))
        {
            numOfKids--;
        }
    }
}
