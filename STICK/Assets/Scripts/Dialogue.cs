using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue : MonoBehaviour {

    public string nameT;

    [TextArea(3, 10)]
    public string[] sentences;

}
