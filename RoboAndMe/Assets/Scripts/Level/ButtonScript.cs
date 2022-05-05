using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attaches to the buttons in the scene
//Can be clicked by Robo and Mimi
public class ButtonScript : MonoBehaviour
{
    private DoorManager dm;
    [SerializeField] private int index;
    private AudioSource source;
    private Sprite startSprite;

    private void Start()
    {
        dm = FindObjectOfType<DoorManager>();
        source = GetComponent<AudioSource>();
        startSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("robo"))
        {
            dm.PushButton(index);
            GetComponent<SpriteRenderer>().sprite = null;
            source.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("robo"))
        {
            dm.ReleaseButton(index);
            GetComponent<SpriteRenderer>().sprite = startSprite;
            source.Play();
        }
    }
}
