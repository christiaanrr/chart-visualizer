using UnityEngine;

public class MedicalEvent
{
    private enum eventType { Symptom, Diagnosis, MedicalAction };
    private eventType type;
    private string title;
    private System.DateTime date;
    private Vector3 site;
    private string siteString;
    private GameController.gameState state;
    private Vector3 specificLocation;
    private string notes;
    private string drugs; // optional
    private string imagePath; // file path of the image
    private Sprite imgSprite; // Sprite of image

    // Default constructor
    public MedicalEvent()
    {

    }

    // if no value for variables, use "" or equivalent
    public MedicalEvent(int eType, string eTitle, int y, int m, int d, GameController.gameState gState, Vector3 location, string note, string drugsAdm, string imgPath)
    {
        type = (eventType)eType;
        title = eTitle;
        date = new System.DateTime(y, m, d);
        state = gState;
        //site = GetSiteVector(place);
        SetSiteString();
        specificLocation = location;
        notes = note;
        drugs = drugsAdm;
        imagePath = imgPath;
        if (imagePath != null)
            imgSprite = Resources.Load<Sprite>(imagePath);
    }

    public void SetSiteString()
    {
        switch (state)
        {
            case GameController.gameState.TORSO_VIEW:
                {
                    siteString = "Torso";
                    break;
                }
            case GameController.gameState.HEAD_NECK_VIEW:
                {
                    siteString = "Head/Neck";
                    break;
                }
            case GameController.gameState.RIGHT_ARM_VIEW:
                {
                    siteString = "Right arm";
                    break;
                }
            case GameController.gameState.LEFT_ARM_VIEW:
                {
                    siteString = "Left arm";
                    break;
                }
            case GameController.gameState.RIGHT_LEG_VIEW:
                {
                    siteString = "Right leg";
                    break;
                }
            case GameController.gameState.LEFT_LEG_VIEW:
                {
                    siteString = "Left leg";
                    break;
                }
        }
    }

    public Vector3 GetSiteVector(string place)
    {
        switch (place)
        {
            case "head":
                break;
            case "left arm":
                break;
            case "right arm":
                break;
            case "torso":
                break;
            case "left leg":
                break;
            case "right leg":
                break;
        }
        return Vector3.zero;
    }

    public string GetInfo()
    {
        //print all info for event
        string allInfo = "Title: " + this.title +
            "\nType: " + this.type +
            "\nDate: " + this.date.ToShortDateString() +
            "\nSite: " + this.siteString +
            "\nNotes: " + this.notes +
            "\nDrugs/treatments: " + this.drugs;
        return allInfo;
    }

    // Getters
    public GameController.gameState GetGameState()
    {
        return this.state;
    }

    public int GetEventType()
    {
        return (int)this.type;
        // Symptom=0 Diagnosis=1 MedicalAction=2
    }

    public string GetEventTitle()
    {
        return this.title;
    }

    public System.DateTime GetEventDate()
    {
        return this.date;
    }

    public string GetEventNotes()
    {
        return this.notes;
    }

    public string GetEventDrugs()
    {
        return this.drugs;
    }

    // returns overall site of medical event (game state view)
    public Vector3 GetSite()
    {
        return this.site;
    }

    // returns site as a string
    public string GetSiteString()
    {
        return this.siteString;
    }

    // returns precise location of medical event
    public Vector3 GetLocation()
    {
        return this.specificLocation;
    }

    // returns Sprite of supplemental image
    public Sprite GetImageSprite()
    {
        return this.imgSprite;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////

    // Setters
    public void SetGameState(GameController.gameState gs)
    {
        this.state = gs;
    }

    public void SetEventType(int t)
    {
        this.type = (eventType)t;
        // Symptom=0 Diagnosis=1 MedicalAction=2
    }

    public void SetEventTitle(string t)
    {
        this.title = t;
    }

    // creates DateTime object for you
    public void SetEventDate(int year, int month, int day)
    {
        this.date = new System.DateTime(year, month, day);
    }

    // use your own DateTime object
    public void SetEventDate(System.DateTime d)
    {
        this.date = d;
    }

    public void SetEventNotes(string n)
    {
        this.notes = n;
    }

    public void SetEventDrugs(string d)
    {
        this.drugs = d;
    }

    // set overall site of medical event (game state view)
    public void SetSite(Vector3 region)
    {
        this.site = region;
    }

    // set precise location of medical event
    public void SetLocation(Vector3 vector)
    {
        this.specificLocation = vector;
    }

    // set Sprite of supplemental image
    public void SetImageSprite(Sprite img)
    {
        this.imgSprite = img;
    }

    public void SetSiteString(string s)
    {
        this.siteString = s;
    }
}