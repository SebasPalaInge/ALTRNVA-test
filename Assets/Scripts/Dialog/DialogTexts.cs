using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogTexts", menuName = "TextObject")]
public class DialogTexts : ScriptableObject
{
    [TextArea] public List<string> textsLines;
}
