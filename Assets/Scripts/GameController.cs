using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    // Variables: specific to testing
    string mainObjectString = "TestCube";
    public float rotationMultiplier = 5.0f;

    // Leap Objects
    LeapListener listener;

    // Gameobjects to track
    GameObject mainObject;

	// Use this for initialization
	void Start () {
        Debug.Log("GameController: RUNNING");

        // Find game objects and assign
        listener = GameObject.Find("LeapListener").GetComponent<LeapListener>();
        mainObject = GameObject.Find(mainObjectString);

        // Verify that objects have been found
        if (listener != null) Debug.Log("Listener: LISTENING");
        if (mainObject != null) Debug.Log("mainObject: IDENTIFIED");
	}
	
	// Update is called once per frame
	void Update () {

        // Rotate object
        // TODO: only if in full body view
        if (listener.rGrabbing)
        {
            mainObject.transform.rotation *=  Quaternion.Euler(0, rotationMultiplier * listener.R_GetPalmVelocity().x, 0);
        }
	}

}
