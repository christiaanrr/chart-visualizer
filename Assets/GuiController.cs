using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiController : MonoBehaviour {
    public string textToWrite = "";
    Camera c;

    float xPos;
    float yPos;
    float w;
    float h;

    public GUIStyle customBox;

    private void Start()
    {
 
        c = GameObject.Find("Main Camera").GetComponent<Camera>();
        xPos = c.pixelWidth* (2.8f/5.0f);
        yPos = c.pixelHeight * (1.0f / 8.0f);
        w = c.pixelWidth * (1.0f / 3.0f);
        h = c.pixelHeight * (5.0f / 8.0f);
    }

    void OnGUI()
    {
        // Make a label that uses the "box" GUIStyle.
        GUI.Label(new Rect(xPos, yPos,  w, h), textToWrite, customBox);

    }

}
