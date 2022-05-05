using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//the script attached to the commandblock object
public class CmdBlock : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private int blockIndex;
    [SerializeField] private CommandType type;
    [SerializeField] private int cost;
    private Vector2 startPos;
    private RectTransform rt;
    Canvas canvas;
    GameObject viewport;
    BlockManager bm;
    private BatteryManager batMan;

    public CommandType Type { get { return type; } }
    public int Cost { get { return cost; } }

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        startPos = rt.anchoredPosition;
        bm = FindObjectOfType<BlockManager>();
        batMan = FindObjectOfType<BatteryManager>();
        canvas = bm.Canvas;
        viewport = bm.Viewport;
    }

    //the following are methods for being dragged by the mouse and placed in content box
    //simply clicking the corresponding button will do the same thing but faster, but it is nice to have options!
    public List<RaycastResult> RaycastMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };
        pointerData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        return results;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent != canvas)
            transform.SetParent(canvas.transform);
        GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> rcm = RaycastMouse();
        foreach (RaycastResult result in rcm)
        {
            if (result.gameObject == viewport)
            {
                transform.SetParent(viewport.transform.GetChild(0).transform);
                return;
            }
        }
        batMan.RemoveBattery(Cost);
        Destroy(gameObject);
    }

    public void Remove()
    {
        batMan.RemoveBattery(Cost);
        Destroy(gameObject);
    }
}
