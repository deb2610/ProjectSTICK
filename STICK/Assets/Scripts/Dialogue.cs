using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue : MonoBehaviour {

    public string nameT;

    [TextArea(3, 10)]
    public string[] sentences;

}
