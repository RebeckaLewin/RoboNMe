using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A scriptable object containing information about its command
[CreateAssetMenu()]
public class Command : ScriptableObject
{
    [SerializeField] private CommandType type;
    [SerializeField] private float value;

    public CommandType Type { get { return type; } set { type = value; } }
    public float Value { get { return value; } set { this.value = value; } }

    public void Init(CommandType type)
    {
        this.type = type;
        value = 0;
    }
}
