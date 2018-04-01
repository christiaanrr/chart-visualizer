using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{

    public Text textBox;
    public GameObject orb;
    public GameController gameController;
    public string textToWrite;


    public void displayText(string str)
    {
        textBox.text = str;
    }

    void Start()
    {
        orb = gameController.selected;
    }

    void Update()
    {
        displayText(textToWrite);
    }


    
}
