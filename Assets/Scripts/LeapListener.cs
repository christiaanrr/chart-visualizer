using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;

public class LeapListener : MonoBehaviour {

    Controller controller;

    // Constant Variables that may need changing
    float normalUpThresh = 0.9f;
    float grabThresh = 0.7f;
    public float summonThresh = 0.9f;
    public float summonPalmNormThresh = 0.7f;
    float leapToWorldMultiplier = 0.5f;

    public bool rInView;        // rh in view
    public bool lInView;        // lh in view
    public bool rSummonReady;      // rh oriented palm down
    public bool lSummonReady;      // lh oriented palm down
    public bool rGrabbing;      // rh grabbing gesture
    public bool lGrabbing;      // lh grabbing gesture

    // advanced
    public bool summonGestureDetected;
    public bool selectionGestureDetected;

    // buttonbool for one time actions
    bool summoning = false;
    bool selecting = false;

    

    // other private leap specific variables
    Frame frame;
    List<Hand> hands;
    Hand rh;
    Hand lh;

	// Use this for initialization
	void Start () {
        controller = new Controller();

        // initialize variables
        rInView = false;
        lInView = false;
	}
	
	// Update is called once per frame
	void Update () {
        frame = controller.Frame();
        hands = frame.Hands;

        // Call methods
        CheckHandVisibility(hands);
        CheckPalmOrientation(hands);
        CheckGrab(hands);
        ListenForSummon(rh);            // RH summon
        ListenForSelection(rh);         // RH selection
        // UpdateRingFingerPos(rh);        // Update the ring finger position .... NOT USING

        // LeapToWorld(rh.PalmPosition);


    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // Rudimentary Gesture Detection: Checking States of Hands
    /////////////////////////////////////////////////////////////////////////////////////////////////////

    /**--------------------------------------------------------------------------------------
     * Check Hand Visibility
     * update rinview and linview to see if they are in the scene
     */
    void CheckHandVisibility(List<Hand> hands)
    {
        rInView = false;
        lInView = false;

        foreach (Hand h in hands)
        {
            if (h.IsRight)
            {
                rInView = true;
                rh = h;
            }
            else if (h.IsLeft)
            {
                lInView = true;
                lh = h;
            }

        }
    }

    /**--------------------------------------------------------------------------------------
     * Check Palm Orientation
     */
    void CheckPalmOrientation(List<Hand> hands)
    {
        rSummonReady = false;
        lSummonReady = false;
        

        foreach (Hand h in hands)
        {
            if (h.PalmNormal.y > summonPalmNormThresh && h.IsRight)
            {
                rSummonReady = true;
            }

            else if (h.PalmNormal.y > summonPalmNormThresh && h.IsLeft)
            {
                rSummonReady = true;
            }
        }
    }

    /**--------------------------------------------------------------------------------------
     * Check if hand grabbing
     */
    void CheckGrab(List<Hand> hands)
    {
        rGrabbing = false;
        lGrabbing = false;

        foreach (Hand h in hands)
        {
            if (h.GrabStrength > grabThresh && h.IsRight)
            {
                rGrabbing = true;
            }

            else if (h.GrabStrength > grabThresh && h.IsLeft)
            {
                lGrabbing = true;
            }
        }
    }

    /**--------------------------------------------------------------------------------------
     * Getter for arm cast
     */
    public Leap.Vector GetArmDir()
    {
        return rh.Arm.Direction * 0.1f ;
    }

    public Leap.Vector GetElbowPos()
    {
        return rh.Arm.ElbowPosition;
    }

    public Leap.Vector GetFingDir()
    {
        return rh.Fingers[4].Direction * 0.1f;
    }

    /**--------------------------------------------------------------------------------------
     * Getters for palm position, normal and velocity for both hands
     */
    public Leap.Vector R_GetPalmPosition()
    {
        //Debug.Log(rh.PalmPosition.x);
        return LeapToWorld(rh.PalmPosition);
    }

    public Leap.Vector R_GetPalmVelocity()
    {
        return rh.PalmVelocity;
    }

    public Leap.Vector R_GetPalmNormal()
    {
        return rh.PalmNormal;
    }

    public Leap.Vector L_GetPalmNormal()
    {
        return lh.PalmNormal;
    }

    public Leap.Vector L_GetPalmVelocity()
    {
        return lh.PalmVelocity;
    }
    

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // Advanced Gesture Detection
    /////////////////////////////////////////////////////////////////////////////////////////////////////

    /**--------------------------------------------------------------------------------------
     * Getters for palm position and velocity for both hands
     */
    void ListenForSummon(Hand rh)
    {
        summonGestureDetected = false;

        if (rh != null)
        {
            // if right palm normal facing upwards
            if (rh.PalmNormal.y > summonPalmNormThresh && rh.GrabStrength > summonThresh && !summoning)
            {
                summonGestureDetected = true;
                summoning = true;
                Debug.Log("Summoning...");
            }

            else if (!(rh.PalmNormal.y >= summonPalmNormThresh && rh.GrabStrength > summonThresh) && summoning)
            {
                summoning = false;
                Debug.Log("No longer summoning");
            }
            
            //Debug.Log(rh.PalmNormal);
        }
    }

    /**--------------------------------------------------------------------------------------
     * Listens for a selection motion and sets selectiong to true
     */
    void ListenForSelection (Hand rh)
    {

        selectionGestureDetected = false;

        if (rh != null)
        {
            // if RH grabbing and the normal isn't indicative of a summon and we aren't currently selecting
            if (rGrabbing && rh.PalmNormal.y < summonPalmNormThresh && !selecting)
            {
                selectionGestureDetected = true;
                selecting = true;
                Debug.Log("Selecting");
            }

            // if RH not in selecting motion and our onetimebool selecting is still true, set it to false
            else if (!(rGrabbing && rh.PalmNormal.y < summonPalmNormThresh) && selecting)
            {
                selecting = false;
                Debug.Log("Not Selecting anymore");
            }
        }
        
    }

    Leap.Vector LeapToWorld (Leap.Vector lv)
    {
        lv.z *= 0.1f;
        InteractionBox iBox = frame.InteractionBox;
        Leap.Vector normalized = iBox.NormalizePoint(lv);
        normalized += new Leap.Vector(0, 0, 0);
        Debug.Log(normalized);
        return normalized * leapToWorldMultiplier;
    }




}
