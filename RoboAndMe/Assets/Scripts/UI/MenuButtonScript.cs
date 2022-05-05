using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Attaches to the menu on the left, changes their sprite according to info from GameManager
public class MenuButtonScript : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    public void ChangeSprite(bool firstSprite)
    {
        Sprite s = null;

        if (firstSprite)
            s = sprites[0];
        else
            s = sprites[1];

        GetComponent<Image>().sprite = s;
    }
}
