using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the battery mechanic
public class BatteryManager : MonoBehaviour
{
    public int maxBattery;
    [SerializeField] private GameObject container, plup;
    public int CurrentBattery { get; set; }

    public void AddBattery(int num)
    {
        for(int i = 0; i < num; i++)
        {
            CurrentBattery++;
            Instantiate(plup, container.transform);
        }
    }

    public void RemoveBattery(int num)
    {
        for (int i = 0; i < num; i++)
        {
            CurrentBattery--;
            Destroy(container.transform.GetChild(i).gameObject);
        }
    }
}
