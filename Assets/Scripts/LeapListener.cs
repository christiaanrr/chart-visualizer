using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;

public class LeapListener : MonoBehaviour {

    Controller controller;

    // Constant Variables that may need changing
    float normalDownThresh = -0.9f;
    float grabThresh = 0.7f;

    public bool rInView;        // rh in view
    public bool lInView;        // lh in view
    public bool rNormalDown;    // rh oriented palm down
    public bool lNormalDown;    // lh oriented palm down
    public bool rGrabbing;      // rh grabbing gesture
    public bool lGrabbing;      // lh grabbing gesture

    

    // other private leap specific variables
    Frame frame;
    List<Hand> hands;

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
            if (h.IsRight) rInView = true;
            else if (h.IsLeft) lInView = true;
        }
    }

    /**--------------------------------------------------------------------------------------
     * Check Palm Orientation
     */
    void CheckPalmOrientation(List<Hand> hands)
    {
        rNormalDown = false;
        lNormalDown = false;

        foreach (Hand h in hands)
        {
            if (h.PalmNormal.y < normalDownThresh && h.IsRight)
            {
                rNormalDown = true;
            }

            else if (h.PalmNormal.y < normalDownThresh && h.IsLeft)
            {
                rNormalDown = true;
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

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // Advanced Gesture Detection
    /////////////////////////////////////////////////////////////////////////////////////////////////////





}
