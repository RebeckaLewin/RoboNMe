using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the animation for the build sign
public class BuildUI : MonoBehaviour
{
    [SerializeField] private GameObject cmdBoard;
    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject goMessage;

    void Start()
    {
        LeanTween.moveLocalY(cmdBoard, 260f, 1f).setEaseOutBounce();
    }

    public void CloseUI()
    {
        LeanTween.moveLocalY(cmdBoard, 500, 0.5f);
        LeanTween.moveLocalY(startBtn, -400, 1f);
        LeanTween.scale(goMessage, Vector3.one, 1f).setEaseOutBounce();
        GetComponent<AudioSource>().Play();
        StartCoroutine(SendAway());
    }

    private IEnumerator SendAway()
    {
        yield return new WaitForSeconds(1.5f);
        LeanTween.moveLocalX(goMessage, 800, 0.5f);
    }
}
