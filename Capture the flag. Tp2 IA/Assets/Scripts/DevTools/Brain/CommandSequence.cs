using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CommandSequence", menuName = "Rival/ComantSequence")]
public class CommandSequence : ScriptableObject
{
    public string commandName;
    public List<Enums.INPUT_BRAIN> secuencias = new List<Enums.INPUT_BRAIN>();
}
