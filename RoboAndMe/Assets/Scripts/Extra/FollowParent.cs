using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An extra script for Mimi when sitting on Robo
//it was never used, but basically resulted in her softly swinging with Robo's movement
public class FollowParent : MonoBehaviour
{
    [SerializeField] private float followSpeed, rotSpeed;
    [SerializeField] private GameObject legs;
    [SerializeField] private GameObject p;
    private float targetX = 0;

    private void Start()
    {
        targetX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectorToTarget = legs.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        q *= Quaternion.Euler(0, 0, 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, rotSpeed);
        targetX = Mathf.Lerp(targetX, p.transform.position.x, followSpeed);
        transform.position = new Vector2(targetX, transform.position.y);
    }
}
