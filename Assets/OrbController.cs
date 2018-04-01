using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour {

	public CameraController cameraController;

	// Use this for initialization
	void Start () {
		cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
	}

	void OnMouseDown() {
		cameraController.buttonPress = true;
	}

	// Update is called once per frame
	void Update () {
		if (cameraController.buttonPress) {
			cameraController.resetRotate();
			cameraController.moveToTarget(cameraController.head.transform);
			cameraController.buttonPress = false;
		}
	}
}