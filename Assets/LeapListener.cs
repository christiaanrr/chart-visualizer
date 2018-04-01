using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;

public class LeapListener : MonoBehaviour
{

    Controller controller;

    // Constant Variables that may need changing
    float normalUpThresh = 0.9f;
    float grabThresh = 0.7f;
    public float summonThresh = 0.9f;
    public float summonPalmNormThresh = 0.7f;
    float leapToWorldMultiplier = 0.5f;
    float backOutPalmThresh = -0.9f;
    float rotatePalmThresh = 0.9f;
    float backOutDistanceThresh = 150.0f;

    public bool rInView;        // rh in view
    public bool lInView;        // lh in view
    public bool rSummonReady;      // rh oriented palm down
    public bool lSummonReady;      // lh oriented palm down
    public bool rGrabbing;      // rh grabbing gesture
    public bool lGrabbing;      // lh grabbing gesture
    public bool r_backOutPositionReady;   // rh sideways palm
    public bool l_autoRotatePosReady;     // lh backout pos ready
    // advanced
    public bool summonGestureDetected = false;
    public bool selectionGestureDetected = false;
    public bool r_backoutGestureDetected = false;
    public bool l_autoRotateGestureDetected = false;


    // buttonbool for one time actions
    bool summoning = false;
    bool selecting = false;

    // gesture related vars
    Leap.Vector backOutInitPos;
    float backOutTravelDistance;
    bool backOutListening = false;

    Leap.Vector autoRotateInitPos;
    float autoRotateTravelDistance;
    bool autoRotateListening = false;



    // other private leap specific variables
    Frame frame;
    List<Hand> hands;
    Hand rh;
    Hand lh;

    // Use this for initialization
    void Start()
    {
        controller = new Controller();

        // initialize variables
        rInView = false;
        lInView = false;
    }

    // Update is called once per frame
    void Update()
    {
        frame = controller.Frame();
        hands = frame.Hands;

        // Call methods
        CheckHandVisibility(hands);
        CheckPalmOrientation(hands);
        CheckGrab(hands);
        ListenForSummon(rh);            // RH summon
        ListenForSelection(rh);         // RH selection
        ListenForBackOut(rh);           // RH backout
        ListenForAutoRotate(lh);        // LH backout motion for auto rotate
        

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
        r_backOutPositionReady = false;


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

            // RH back out gesture
            if (h.PalmNormal.x < backOutPalmThresh && h.IsRight)
            {
                r_backOutPositionReady = true;
                if (!backOutListening)
                {
                    backOutInitPos = h.PalmPosition;
                    backOutListening = true;
                }

            }

            // LH autoRotate gesture
            if (h.PalmNormal.x > rotatePalmThresh && h.IsLeft)
            {
                l_autoRotatePosReady = true;
                if (!autoRotateListening)
                {
                    autoRotateInitPos = h.PalmPosition;
                    autoRotateListening = true;
                }

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
        return rh.Arm.Direction * 0.05f;
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
    void ListenForSelection(Hand rh)
    {
        

        if (rh != null && rh.PalmNormal.y < summonPalmNormThresh - 0.1f)
        {
            // if RH grabbing and the normal isn't indicative of a summon and we aren't currently selecting
            if (rGrabbing && rh.PalmNormal.y < summonPalmNormThresh && !selecting)
            {
                selectionGestureDetected = true;
                selecting = true;
                Debug.Log("Selecting");
            }

            // if RH not in selecting motion and our onetimebool selecting is still true, set it to false
            else if (!rGrabbing || rh.PalmNormal.y >= summonPalmNormThresh)
            {
                selecting = false;
                Debug.Log("Not Selecting anymore");
            }
        }

    }

    /**--------------------------------------------------------------------------------------
     * Listens for a backout motion and sets backout to true RH
     */
    void ListenForBackOut(Hand rh)
    {

        r_backoutGestureDetected = false;

        if (rInView && r_backOutPositionReady)
        {
            backOutListening = true;
            backOutTravelDistance = Mathf.Abs(rh.PalmPosition.x - backOutInitPos.x);
        }

        else if (!(rInView && r_backOutPositionReady))
        {
            backOutListening = false;
            backOutTravelDistance = 0;
        }

        if (backOutTravelDistance > backOutDistanceThresh)
        {
            Debug.Log("backout!!!");
            r_backoutGestureDetected = true;
        }

         // Debug.Log(backOutTravelDistance);
          


    }


    /**--------------------------------------------------------------------------------------
     * Listens for a backout motion and sets backout to true LH
     */
    void ListenForAutoRotate(Hand lh)
    {

        l_autoRotateGestureDetected = false;

        if (lInView && l_autoRotatePosReady)
        {
            autoRotateListening = true;
            autoRotateTravelDistance = Mathf.Abs(lh.PalmPosition.x - autoRotateInitPos.x);
        }

        else if (!(lInView && l_autoRotatePosReady))
        {
            autoRotateListening = false;
            autoRotateTravelDistance = 0;
        }

        if (autoRotateTravelDistance > backOutDistanceThresh)
        {
            // Debug.Log("rotate!!!");
            l_autoRotateGestureDetected = true;
        }

        // Debug.Log(backOutTravelDistance);



    }


    Leap.Vector LeapToWorld(Leap.Vector lv)
    {
        lv.z *= 0.1f;
        InteractionBox iBox = frame.InteractionBox;
        Leap.Vector normalized = iBox.NormalizePoint(lv);
        normalized += new Leap.Vector(0, 0, 0);
        Debug.Log(normalized);
        return normalized * leapToWorldMultiplier;
    }




}
