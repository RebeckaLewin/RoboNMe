using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages the spawning of blocks, and keep track of which block to add
public class BlockManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> blocks;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject viewport;
    [SerializeField] private BatteryManager batMan;

    public Canvas Canvas { get { return canvas; } }

    public GameObject Viewport { get { return viewport; } }

    private void Start()
    {
        batMan = FindObjectOfType<BatteryManager>();
    }

    public void SpawnBlock(int index)
    {
        //if battery is full, no block will be added
        if(batMan.CurrentBattery != batMan.maxBattery)
        {
            Instantiate(blocks[index], blocks[index].transform.position / canvas.scaleFactor, transform.rotation, content.transform);
            batMan.AddBattery(blocks[index].GetComponent<CmdBlock>().Cost);
        }
    }

    public List<GameObject> Blocks()
    {
        return blocks;
    }
}
