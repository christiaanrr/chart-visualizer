using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    /* Change this to a list of patients 
       then just initialize them as you go */
    // private List<Patient> patients;
    private Patient p1;
    public enum gameState { FULL_BODY_VIEW, TORSO_VIEW, HEAD_NECK_VIEW, RIGHT_ARM_VIEW, LEFT_ARM_VIEW, RIGHT_LEG_VIEW, LEFT_LEG_VIEW };
    private List<MedicalEvent> p1_events = new List<MedicalEvent>();
    public GameObject redOrb;
    public GameObject blueOrb;
    public GameObject yellowOrb;
    public List<MedicalOrb> orbList = new List<MedicalOrb>();
    private gameState previousState = gameState.FULL_BODY_VIEW;
    private DateTime previousEndPoint = new DateTime(2018, 04, 01);// current day

    public gameState currentGameState;

    public float rotationMultiplier = -0.01f;
    public bool objectIsSelected = false;
    public GameObject leapController;

    public TextController textController;
    public GuiController gController;

    // Leap Objects
    LeapListener listener;

    // Gameobjects to track
    public GameObject mainObject;
    public Camera camera;
    public CameraController cameraController;
    public GameObject selected;

    // Temporary list of events when not in full body view
    public List<GameObject> orbsInView;


    // Use this for initialization
    void Start()
    {
        InitEvents();
        // initialize patient
        p1 = new Patient("Joe Bruin", 19836220, 1959, 08, 05, 'M', p1_events);

        DateTime temp = new DateTime(2020, 12, 31);
        Debug.Log(p1.GetInfo());
        Debug.Log(p1.GetEventsUpTo(temp)[14].GetInfo());
        DisplayOrbs();


        leapController = GameObject.Find("LeapHandController");
        listener = GameObject.Find("LeapListener").GetComponent<LeapListener>();
        mainObject = GameObject.Find("patient");
        gController = GameObject.Find("GUIController").GetComponent<GuiController>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();


        // Verify that objects have been found
        if (listener != null) Debug.Log("Listener: LISTENING");
        if (mainObject != null) Debug.Log("mainObject: IDENTIFIED");


        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        currentGameState = gameState.FULL_BODY_VIEW;
    }

    private void InitEvents()
    {
        // Initialize events and list for demo purposes. Symptom=0 Diagnosis=1 MedicalAction=2
        MedicalEvent d0 = new MedicalEvent(1, "Strep Throat", 1985, 03, 09, gameState.HEAD_NECK_VIEW, new Vector3(-5.23f, 7.837f, 0.236f), "Throat appeared inflamed and red. Patient complained of congestion, cough, and pain. Throat swab confirmed case of strep throat.", "Azithromycin", null);
        MedicalEvent e0 = new MedicalEvent(2, "Lasik Eye Surgery", 1991, 02, 14, gameState.HEAD_NECK_VIEW, new Vector3(-5.298f, 8.37f, -0.631f), "Laser eye surgery.", "", null);
        MedicalEvent e1 = new MedicalEvent(1, "Fractured Radius", 2004, 01, 20, gameState.RIGHT_ARM_VIEW, new Vector3(-6.292f, 5.698f, -1.05f), "Cause: Bird scooter accident.", "Over-the-counter pain medication recommended", "fracture-bone-2333164_960_720");
        MedicalEvent e2 = new MedicalEvent(0, "Shin Splints", 2008, 11, 17, gameState.RIGHT_LEG_VIEW, new Vector3(-5.45f, 2.388f, -0.66f), "Complaints of sharp pain through right shin following heavy running.", "None", null);
        MedicalEvent e3 = new MedicalEvent(0, "Shin Splints", 2009, 09, 24, gameState.RIGHT_LEG_VIEW, new Vector3(-5.487f, 2.076f, -0.587f), "Second case of sharp pain through right shin following heavy running.", "None", null);
        MedicalEvent e4 = new MedicalEvent(1, "Shin Splint Diagnosis", 2009, 09, 24, gameState.RIGHT_LEG_VIEW, new Vector3(-5.504f, 1.696f, -0.502f), "Doctor diagnosis of shin splints.", "Ice and over-the-counter pain medication recommended", null);
        MedicalEvent e5 = new MedicalEvent(0, "Lower Abdomen Pain", 2015, 10, 13, gameState.TORSO_VIEW, new Vector3(-4.454f, 5.375f, -0.418f), "ER visit 10/14/2015", "None", null);
        MedicalEvent e6 = new MedicalEvent(2, "Colonoscopy", 2015, 10, 14, gameState.TORSO_VIEW, new Vector3(-4.83f, 5.41f, 0.42f), "Colonoscopy reveals malignant growth.", "Cancer treatment plan pending", null);
        MedicalEvent e7 = new MedicalEvent(2, "Surgical Tumor Removal", 2015, 11, 12, gameState.TORSO_VIEW, new Vector3(-4.52f, 5.91f, -0.47f), "Removal of abnormal mass in colon.", "Anesthetic administered", null);
        MedicalEvent e8 = new MedicalEvent(1, "Spread of Cancer to Underarm Lymph Nodes", 2016, 08, 19, gameState.TORSO_VIEW, new Vector3(-4.23f, 6.92f, -0.19f), "CT Scan reveals growths in underarm area.", "Chemotherapy plan in motion", null);
        MedicalEvent e9 = new MedicalEvent(2, "Chemotherapy Administration", 2016, 09, 01, gameState.TORSO_VIEW, new Vector3(-5.298f, 7.25f, -0.643f), "First of four treatments.", "Doxorubicin", null);
        MedicalEvent e10 = new MedicalEvent(2, "Chemotherapy Administration", 2016, 10, 01, gameState.TORSO_VIEW, new Vector3(-4.999f, 7.097f, -0.603f), "Second of four treatments.", "Doxorubicin", null);
        MedicalEvent e11 = new MedicalEvent(2, "Chemotherapy Administration", 2016, 11, 01, gameState.TORSO_VIEW, new Vector3(-5.125f, 6.931f, -0.778f), "Third of four treatments.", "Doxorubicin", null);
        MedicalEvent e12 = new MedicalEvent(0, "Joint Pain in Hand", 2017, 11, 18, gameState.LEFT_ARM_VIEW, new Vector3(-2.91f, 4.76f, 0.58f), "Doctor visit for chronic joint pain.", "None", null);
        MedicalEvent e13 = new MedicalEvent(1, "Arthritis Diagnosis", 2017, 11, 18, gameState.LEFT_ARM_VIEW, new Vector3(-3.09f, 5.15f, 0.6f), "Patient diagnosed with arthritis.", "Aleve", null);

        p1_events.Add(d0);
        p1_events.Add(e0);
        p1_events.Add(e1);
        p1_events.Add(e2);
        p1_events.Add(e3);
        p1_events.Add(e4);
        p1_events.Add(e5);
        p1_events.Add(e6);
        p1_events.Add(e7);
        p1_events.Add(e8);
        p1_events.Add(e9);
        p1_events.Add(e10);
        p1_events.Add(e11);
        p1_events.Add(e12);
        p1_events.Add(e13);
    }

    // Update is called once per frame
    void Update()
    {

       // textController.displayText("LOL");

        // gameState current = gameState.TORSO_VIEW; // gets updated by Ryan
        DateTime endDate = new DateTime(2000, 12, 12); // gets updated by Ryan maybe


        



        switch (currentGameState)
        {
            case gameState.FULL_BODY_VIEW:
                {
                    gController.textToWrite = p1.GetInfo();
                    if (previousState != currentGameState)
                    {
                        selected = null;
                        UpdateOrbs(gameState.FULL_BODY_VIEW);
                    }

                    // ---------------------------------------------------------------------------------------------------------------------
                    // Rotate object: if LH grabs
                    // TODO: only if in full body view
                    if (listener.lGrabbing)
                    {
                        mainObject.transform.rotation *= Quaternion.Euler(0, rotationMultiplier * listener.L_GetPalmVelocity().x, 0);
                    }


                    // ---------------------------------------------------------------------------------------------------------------------
                    // TODO: Backout
                    if (listener.r_backoutGestureDetected)
                    {
                        cameraController.resetCamera();
                        currentGameState = gameState.FULL_BODY_VIEW;
                    }


                    break;
                }
            case gameState.TORSO_VIEW:
                {
                    if (previousState != currentGameState)
                    {
                        // Debug.Log("calling update orbs");
                        UpdateOrbs(gameState.TORSO_VIEW);
                        cameraController.moveToTarget(cameraController.torso.transform);
                    }


                    // ---------------------------------------------------------------------------------------------------------------------
                    // TODO: Backout
                    if (listener.r_backoutGestureDetected)
                    {
                        cameraController.resetCamera();
                        currentGameState = gameState.FULL_BODY_VIEW;
                    }

                    break;
                }
            case gameState.HEAD_NECK_VIEW:
                {
                    if (previousState != currentGameState)
                    {
                        UpdateOrbs(gameState.HEAD_NECK_VIEW);
                        cameraController.moveToTarget(cameraController.head.transform);
                    }


                    // ---------------------------------------------------------------------------------------------------------------------
                    // TODO: Backout
                    if (listener.r_backoutGestureDetected)
                    {
                        cameraController.resetCamera();
                        currentGameState = gameState.FULL_BODY_VIEW;
                    }

                    break;
                }
            case gameState.RIGHT_ARM_VIEW:
                {
                    if (previousState != currentGameState)
                    {
                        UpdateOrbs(gameState.RIGHT_ARM_VIEW);
                        cameraController.moveToTarget(cameraController.rightArm.transform);
                    }


                    // ---------------------------------------------------------------------------------------------------------------------
                    // TODO: Backout
                    if (listener.r_backoutGestureDetected)
                    {
                        cameraController.resetCamera();
                        currentGameState = gameState.FULL_BODY_VIEW;
                    }

                    break;
                }
            case gameState.LEFT_ARM_VIEW:
                {
                    if (previousState != currentGameState)
                    {
                        UpdateOrbs(gameState.LEFT_ARM_VIEW);
                        cameraController.moveToTarget(cameraController.leftArm.transform);

                    }

                    // ---------------------------------------------------------------------------------------------------------------------
                    // TODO: Backout
                    if (listener.r_backoutGestureDetected)
                    {
                        cameraController.resetCamera();
                        currentGameState = gameState.FULL_BODY_VIEW;
                    }

                    break;
                }
            case gameState.RIGHT_LEG_VIEW:
                {
                    if (previousState != currentGameState)
                    {
                        UpdateOrbs(gameState.RIGHT_LEG_VIEW);
                        cameraController.moveToTarget(cameraController.legs.transform);
                    }

                    // ---------------------------------------------------------------------------------------------------------------------
                    // TODO: Backout
                    if (listener.r_backoutGestureDetected)
                    {
                        cameraController.resetCamera();
                        currentGameState = gameState.FULL_BODY_VIEW;
                    }


                    break;
                }
            case gameState.LEFT_LEG_VIEW:
                {
                    if (previousState != currentGameState)
                    {
                        UpdateOrbs(gameState.LEFT_LEG_VIEW); 
                        cameraController.moveToTarget(cameraController.legs.transform);
                    }

                    // ---------------------------------------------------------------------------------------------------------------------
                    // TODO: Backout
                    if (listener.r_backoutGestureDetected)
                    {
                        cameraController.resetCamera();
                        currentGameState = gameState.FULL_BODY_VIEW;
                    }


                    break;
                }
        }

        if (currentGameState != gameState.FULL_BODY_VIEW)
        {
            if (previousState != currentGameState)
            {
                foreach (var o in orbList)
                {

                }
            }
        }


        previousState = currentGameState;
    }



    private void FixedUpdate()
    {

        // ---------------------------------------------------------------------------------------------------------------------
        // RayCast from palm position
        if (listener.rInView)
        {
            float shaving = 0.5f;
            Vector3 rayMod = new Vector3(0, 0.2f, -0.5f);
            Vector3 rayDir = new Vector3(listener.GetArmDir().x * shaving, listener.GetArmDir().y, listener.GetArmDir().z * -1.0f);

            Ray r = new Ray(leapController.transform.position + rayMod, rayDir * 300.0f);
            Debug.DrawRay(leapController.transform.position + rayMod, rayDir * 300.0f);

            RaycastHit hit;
            if (Physics.Raycast(r, out hit))
            {
                if (currentGameState == gameState.FULL_BODY_VIEW && hit.collider.tag == "zoom-zone")
                {
                    Debug.Log("HIT");
                    if (listener.selectionGestureDetected)
                    {
                        selected = hit.collider.gameObject;
                    }
                }
                
                /*
                if (currentGameState != gameState.FULL_BODY_VIEW && (hit.collider.name == "Diag.Orb(Clone)" || 
                                                                    hit.collider.name == "Sym.Orb(Clone)" || hit.collider.name == "Op.Orb(Clone)"))
                {
                    Debug.Log("askdjflkjwef");
                }
                */
                
                if (currentGameState != gameState.FULL_BODY_VIEW && hit.collider.tag == "orb")
                {
                    if (listener.selectionGestureDetected)
                    {
                        selected = hit.collider.gameObject;
                        Debug.Log(selected);
                    }
                }



            }


        }

        if (selected != null)
        {

            if (currentGameState != gameState.FULL_BODY_VIEW && selected.tag == "orb")
            {
                gController.textToWrite = selected.GetComponent<MedicalOrb>().GetMedicalEvent().GetInfo();
            }
            // ONLY HAPPENS if in full body view. There is no summoning otherwise
            if (listener.rInView && currentGameState == gameState.FULL_BODY_VIEW)
            {

                Debug.Log("Selected an object!!!!!!!!!!!!!!!!!");

                if (listener.summonGestureDetected)
                {
                    // CALL CHRISTIAN FUNC
                    switch (selected.name)
                    {
                        case "HeadCollider":
                            {
                                currentGameState = gameState.HEAD_NECK_VIEW;
                                Debug.Log("changed game state");
                                if (selected.tag != "orb") selected = null;
                                break;
                            }

                        case "TorsoCollider":
                            {
                                currentGameState = gameState.TORSO_VIEW;
                                Debug.Log("changed game state");
                                if (selected.tag != "orb") selected = null;
                                break;
                            }

                        case "RightArmCollider":
                            {
                                currentGameState = gameState.RIGHT_ARM_VIEW;
                                Debug.Log("changed game state");
                                if (selected.tag != "orb") selected = null;
                                break;
                            }
                        case "LeftArmCollider":
                            {
                                currentGameState = gameState.LEFT_ARM_VIEW;
                                Debug.Log("changed game state");
                                if (selected.tag != "orb") selected = null;
                                break;
                            }
                        case "LeftLegCollider":
                            {
                                currentGameState = gameState.LEFT_LEG_VIEW;
                                Debug.Log("changed game state");
                                if (selected.tag != "orb") selected = null;
                                break;
                            }
                        case "RightLegCollider":
                            {
                                currentGameState = gameState.RIGHT_LEG_VIEW;
                                Debug.Log("changed game state");
                                if (selected.tag != "orb") selected = null;
                                break;
                            }
                    }
                }
            }
        }




    }



    /*
     *  state: gameState enum value
     *  date: time interval end limit
     */
    private void UpdateOrbs(gameState state)
    {
        foreach (var medOrb in orbList)
        {
            Debug.Log("state: " + state + " event: " + medOrb.GetComponent<MedicalOrb>().GetMedicalEvent().GetGameState());
            if (state == medOrb.GetComponent<MedicalOrb>().GetMedicalEvent().GetGameState())
            {
                Debug.Log("setting visible");
                medOrb.GetComponent<MeshRenderer>().enabled = true;
                medOrb.GetComponent<Collider>().enabled = true;
                // medOrb.transform.localScale *= 2.0f;
            }
            else
            {
                medOrb.GetComponent<MeshRenderer>().enabled = false;
                medOrb.GetComponent<Collider>().enabled = false;
            }
        }
    }

    private void UpdateOrbs(DateTime date)
    {
        foreach (var medOrb in orbList)
        {
            if (DateTime.Compare(medOrb.GetComponent<MedicalOrb>().GetMedicalEvent().GetEventDate(), date) >= 0)
            {

            }
        }
    }

    private void DisplayOrbs()
    {
        // replace vectors with the correct medical event location vector medEvent.GetLocation()
        GameObject medOrb;
        foreach (var medEvent in p1.GetEvents())
        {
            if (medEvent.GetEventType() == 0) // symptom
            {
                medOrb = Instantiate(yellowOrb, medEvent.GetLocation(), Quaternion.identity, this.transform);
                medOrb.GetComponent<MedicalOrb>().SetMedicalEvent(medEvent);
                medOrb.GetComponent<MedicalOrb>().SetColor("yellow");
                orbList.Add(medOrb.GetComponent<MedicalOrb>());
                medOrb.AddComponent<OrbController>();
            }
            else if (medEvent.GetEventType() == 1) // diagnosis
            {
                medOrb = Instantiate(blueOrb, medEvent.GetLocation(), Quaternion.identity, this.transform);
                medOrb.GetComponent<MedicalOrb>().SetMedicalEvent(medEvent);
                medOrb.GetComponent<MedicalOrb>().SetColor("blue");
                orbList.Add(medOrb.GetComponent<MedicalOrb>());
                medOrb.AddComponent<OrbController>();
            }
            else if (medEvent.GetEventType() == 2) // medical action
            {
                medOrb = Instantiate(redOrb, medEvent.GetLocation(), Quaternion.identity, this.transform);
                medOrb.GetComponent<MedicalOrb>().SetMedicalEvent(medEvent);
                medOrb.GetComponent<MedicalOrb>().SetColor("red");
                orbList.Add(medOrb.GetComponent<MedicalOrb>());
                medOrb.AddComponent<OrbController>();
            }
        }
    }

    void HighLightRegions()
    {
        
    }
}