using UnityEngine;

public class CameraController : MonoBehaviour {

	public Vector3 originalCamPos;

	public GameObject head;
	public GameObject torso;
	public GameObject legs;
	public GameObject leftArm;
	public GameObject rightArm;
	public GameObject rotateCube;
	public GameObject medOrb;

	public float viewDistance = 10.0f;

	public bool buttonPress = false;

	public void moveToTarget(Transform target) {
		Vector3 sourcePos = transform.position;
		Vector3 destPos = target.position - transform.forward * viewDistance;
		float i = 0.0f;
		while (i < 1.0f) {
			transform.position = Vector3.Lerp(sourcePos, destPos, Mathf.SmoothStep(0,1,i));
			i += Time.deltaTime;
		}
	}

	public void resetCamera() {
		Camera.main.transform.position = originalCamPos;
	}

	void rotateCameraRight(Transform target) {
		transform.RotateAround (target.transform.position, Vector3.down, 120 * Time.deltaTime);
	}

	void rotateCameraLeft(Transform target) {
		transform.RotateAround (target.transform.position, Vector3.up, 120 * Time.deltaTime);
	}

	public void resetRotate() {
		transform.rotation = Quaternion.identity;
	}

	void OnMouseDown() {
		buttonPress = true;
	}

	void Start () {
		originalCamPos = Camera.main.transform.position;
		head = GameObject.Find("head");
		torso = GameObject.Find("torso");
		legs = GameObject.Find("legs");
		leftArm = GameObject.Find("leftArm");
		rightArm = GameObject.Find("rightArm");
		medOrb = GameObject.Find("medOrb");
		rotateCube = GameObject.Find("rotateCube");

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("i")) {
			resetRotate();
			moveToTarget(head.transform);

		}
        /*
		if (Input.GetKey ("o")) {
			resetRotate();
			moveToTarget(torso.transform);
		}
        */

        if (Input.GetKey("o"))
        {
            resetRotate();
            moveToTarget(head.transform);
        }


        if (Input.GetKey ("r")) {
			resetRotate();
			resetCamera ();
		}
		if (Input.GetKey ("h")) {
			resetRotate();
			moveToTarget(leftArm.transform);
		}
		if (Input.GetKey ("j")) {
			resetRotate();
			moveToTarget(rightArm.transform);
		}
		if (Input.GetKey ("k")) {
			resetRotate();
			moveToTarget(legs.transform);
		}
		if (Input.GetKey ("m")) {
			rotateCameraRight(rotateCube.transform);
		}
		if (Input.GetKey ("n")) {
			rotateCameraLeft(rotateCube.transform);
		}
		if (buttonPress) {
			resetRotate();
			moveToTarget(head.transform);
		}
	}
}