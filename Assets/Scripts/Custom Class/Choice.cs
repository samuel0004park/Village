using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Choice 
{
    [TextArea(1, 2)]
    public string question;
    public string[] answers;//max 1~4
}
