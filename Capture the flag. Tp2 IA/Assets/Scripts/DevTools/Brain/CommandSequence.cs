using System.Collections.Generic;
using UnityEngine;
using DevTools.Enums;

public class CommandSequence : ScriptableObject
{
    public string commandName;
    public List<Enums.INPUT_BRAIN> secuencias = new List<Enums.INPUT_BRAIN>();
}
