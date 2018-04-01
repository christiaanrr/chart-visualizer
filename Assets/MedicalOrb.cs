using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalOrb : MonoBehaviour
{
    MedicalEvent m = new MedicalEvent(); // blank constructor previously created
    string color = ""; // set equal to "red", "yellow", "blue"

    void Start()
    {
        Debug.Log("hello");
    }

    public void SetColor(string clr)
    {
        color = clr;
    }

    public void SetMedicalEvent(MedicalEvent mymed)
    {
        m.SetEventType(mymed.GetEventType());
        m.SetEventTitle(mymed.GetEventTitle());
        m.SetEventDate(mymed.GetEventDate());
        m.SetEventNotes(mymed.GetEventNotes());
        m.SetEventDrugs(mymed.GetEventDrugs());
        m.SetLocation(mymed.GetLocation());
        m.SetImageSprite(mymed.GetImageSprite());
        m.SetSite(mymed.GetSite());
        m.SetSiteString(mymed.GetSiteString());
        m.SetGameState(mymed.GetGameState());
    }

    public MedicalEvent GetMedicalEvent()
    {
        return m;
    }

    // Update is called once per frame
    void Update()
    {

    }


}