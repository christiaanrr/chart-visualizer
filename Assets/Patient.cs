using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient
{
    // public string firstName;
    // public string lastName;
    private string fullName; // = firstName + " " + lastName; 
    private int mrn; //medical record number
    private System.DateTime dob; // date of birth
    private char gender; // gender (M, F, O)
    private List<MedicalEvent> medicalEvents; // medical events list

    /* name: string name
     * mrNum: MRN 
     * y: Birth year
     * m: Birth month
     * d: Birth day
     * g: Gender char 'M' 'F' 'O'
     * events: List<MedicalEvent> of patient's events
     */
    public Patient(string name, int mrNum, int y, int m, int d, char g, List<MedicalEvent> events)
    {
        fullName = name;
        mrn = mrNum;
        dob = new System.DateTime(y, m, d);
        gender = g;
        medicalEvents = events;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetInfo()
    {
        //print all info for event
        string allInfo = "Name: " + this.fullName +
            "\nMRN: " + this.mrn +
            "\nDOB: " + this.dob.ToShortDateString() +
            "\nGender: " + this.gender;
        return allInfo;
    }

    // Get list of events up to certain date
    public List<MedicalEvent> GetEventsUpTo(System.DateTime toDate)
    {
        List<MedicalEvent> returnEvents = new List<MedicalEvent>();
        foreach (var medEvent in this.medicalEvents)
        {
            if (System.DateTime.Compare(medEvent.GetEventDate(), toDate) <= 0)
            {
                returnEvents.Add(medEvent);
            }
            else break;
        }
        return returnEvents;
    }

    // Get list of events since a certain date
    public List<MedicalEvent> GetEventsSince(System.DateTime fromDate)
    {
        List<MedicalEvent> returnEvents = new List<MedicalEvent>();
        foreach (var medEvent in this.medicalEvents)
        {
            if (System.DateTime.Compare(medEvent.GetEventDate(), fromDate) >= 0)
            {
                returnEvents.Add(medEvent);
            }
            else break;
        }
        return returnEvents;
    }

    // Get list of events between start and end dates (inclusive)
    public List<MedicalEvent> GetEventsWithin(System.DateTime start, System.DateTime end)
    {
        List<MedicalEvent> returnEvents = new List<MedicalEvent>();
        foreach (var medEvent in this.medicalEvents)
        {
            if (System.DateTime.Compare(medEvent.GetEventDate(), start) >= 0 &&
                System.DateTime.Compare(medEvent.GetEventDate(), end) <= 0)
            {
                returnEvents.Add(medEvent);
            }
            else break;
        }
        return returnEvents;
    }

    // Getters
    public string GetName()
    {
        return this.fullName;
    }

    public int GetMRN()
    {
        return this.mrn;
    }

    public System.DateTime GetDOB()
    {
        return this.dob;
    }

    public char GetGender()
    {
        return this.gender;
    }

    public List<MedicalEvent> GetEvents()
    {
        return this.medicalEvents;
    }
}
