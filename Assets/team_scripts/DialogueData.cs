
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public string characterName;
    public Sprite portrait;
    [TextArea(3, 6)]
    public string[] dialogueLines;
}
