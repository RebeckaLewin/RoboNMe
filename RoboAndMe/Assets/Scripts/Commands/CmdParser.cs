using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Translates the commands boxes placed in the UI box to actual commands, and sends the completed list to Robo
public class CmdParser : MonoBehaviour
{
    private RoboManager rm;
    private List<Command> commands = new List<Command>();
    [SerializeField] private GameObject content;

    private void Start()
    {
        rm = FindObjectOfType<RoboManager>();
    }

    public void StartProgram()
    {
        Parse();
        rm.CreateListAndStart(commands);
    }

    private void Parse()
    {
        commands.Clear();
        foreach(Transform child in content.transform)
        {
            Command cmd = ScriptableObject.CreateInstance<Command>();
            cmd.Init(child.GetComponent<CmdBlock>().Type);
            switch (cmd.Type)
            {
                case CommandType.shortDelay:
                    cmd.Value = 2;
                    break;
                case CommandType.longDelay:
                    cmd.Value = 5;
                    break;
            }
            commands.Add(cmd);
        }
    }
}
