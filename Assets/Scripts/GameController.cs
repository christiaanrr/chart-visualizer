using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    // Variables: specific to testing
    string mainObjectString = "TestCube";
    GameObject leapController;
    Camera camera;

    // vars for gestures 
    public float rotationMultiplier = -0.01f;
    bool objectIsSelected = false;                                 

    // Leap Objects
    LeapListener listener;

    // Gameobjects to track
    GameObject mainObject;

	// Use this for initialization
	void Start () {
        Debug.Log("GameController: RUNNING");

        // Find game objects and assign
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        leapController = GameObject.Find("LeapHandController");
        listener = GameObject.Find("LeapListener").GetComponent<LeapListener>();
        mainObject = GameObject.Find(mainObjectString);

        // Verify that objects have been found
        if (listener != null) Debug.Log("Listener: LISTENING");
        if (mainObject != null) Debug.Log("mainObject: IDENTIFIED");
        
	}
	
	// Update is called once per frame
	void Update () {

        // ---------------------------------------------------------------------------------------------------------------------
        // Rotate object: if LH grabs
        // TODO: only if in full body view
        if (listener.lGrabbing)
        {
            mainObject.transform.rotation *=  Quaternion.Euler(0, rotationMultiplier * listener.L_GetPalmVelocity().x, 0);
        }



        // ---------------------------------------------------------------------------------------------------------------------
        // TODO: Summon motion: Object selected
        // TODO: need to make sure that an object is selected


        
    }



    GameObject selected;

    private void FixedUpdate()
    {

        // ---------------------------------------------------------------------------------------------------------------------
        // RayCast from palm position
        if (listener.rInView)
        {
            float shaving = 0.5f;
            Vector3 rayMod = new Vector3(0, 0.2f, -0.5f);
            Vector3 rayDir = new Vector3(listener.GetArmDir().x * shaving, listener.GetArmDir().y, listener.GetArmDir().z * -1.0f);

            Ray r = new Ray(leapController.transform.position + rayMod, rayDir * 200.0f);
            Debug.DrawRay(leapController.transform.position + rayMod, rayDir * 200.0f);

            RaycastHit hit;
            if (Physics.Raycast(r, out hit))
            {
                if (hit.collider.tag == "orb")
                {
                    if (listener.selectionGestureDetected)
                    {
                        selected = hit.collider.gameObject;
                    }
                }
            }
            
            
        }

        if (selected != null)
        { 
      
            if (listener.rInView)
            {
                if (listener.summonGestureDetected)
                {
                    selected.transform.position = camera.transform.position + new Vector3(0,0,2.0f);
                }
            }
        }

        
       




    }

    // leap vector to unity vector
    Vector3 lv_t_uv(Leap.Vector lv)
    {
        return new Vector3(lv.x, lv.y, lv.z);
    }

   

}
