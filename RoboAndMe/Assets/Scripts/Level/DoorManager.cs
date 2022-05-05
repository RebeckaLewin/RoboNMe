using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the door mechanic, opens the door if all respective buttons are pressed
public class DoorManager : MonoBehaviour
{
    [SerializeField] private bool[] trigger;

    public void PushButton(int index)
    {
        trigger[index] = true;

        foreach(bool t in trigger)
        {
            if(t == false)
            {
                return;
            }
        }
        Open();
        GetComponent<AudioSource>().Play();
    }

    public void ReleaseButton(int index)
    {
        trigger[index] = false;
    }

    private void Open()
    {
        LeanTween.moveLocalY(this.gameObject, 3f, 3f).setEaseInOutCirc();
    }
}
