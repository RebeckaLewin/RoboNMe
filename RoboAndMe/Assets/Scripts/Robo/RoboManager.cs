using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The class for handling Robo's internal logic
//During play phase, calls Robo's action methods in sequence based on the commandlist
public class RoboManager : MonoBehaviour
{
    private List<Command> commands = new List<Command>();
    private RoboScript robo;
    private static int currentCommand = 0;
    private float currentDelay = 0;
    [SerializeField] private GameObject arrow, content;

    private void Start()
    {
        robo = GetComponent<RoboScript>();
        currentCommand = 0;
    }

    public void SendCommand()
    {
        if (currentCommand < commands.Count)
        {
            bool go = false;
            switch (commands[currentCommand].Type)
            {
                case CommandType.move:
                    go = robo.Move();
                    break;
                case CommandType.turn:
                    robo.Turn();
                    go = true;
                    break;
                case CommandType.jump:
                    robo.Jump();
                    break;
                default:
                    robo.Delay();
                    currentDelay = commands[currentCommand].Value;
                    StartCoroutine(Delay());
                    Debug.Log(currentDelay);
                    break;
            }
            MoveArrow();
            currentCommand++;
            if (go)
            {
                SendCommand();
            }
        }
    }

    //gets command lists from parser and starts the program
    public void CreateListAndStart(List<Command> list)
    {
        GameManager.State = GameStates.playPhase;
        commands.Clear();
        foreach (Command cmd in list)
        {
            commands.Add(cmd);
        }
        arrow.SetActive(true);
        MoveArrow();
        SendCommand();
    }

    private void MoveArrow()
    {
        if(currentCommand < commands.Count)
        {
            Vector2 targetPos = new Vector2(arrow.transform.position.x, content.transform.GetChild(currentCommand).transform.position.y);
            arrow.transform.position = targetPos;
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(currentDelay);
        SendCommand();
    }
}
