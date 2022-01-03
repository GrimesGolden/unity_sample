using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningCurve : MonoBehaviour
{
    // Chapter 4 variables start here
    int DiceRoll = 7;
    int CharacterGold = 100;
    int PlayerLives = 3;

    // Chapter 5 adding component variables.
    public Transform CamTransform;
    public GameObject DirectionLight;
    public Transform LightTransform;


    // Start is called automatically before the first frame update
    void Start()
    {
        // Creating a new struct (hammer) and using it in knight.
        //Weapon hammer = new Weapon("Hammer", 100);

        //Paladin knight = new Paladin("Uther the LightBringer", hammer);
        //knight.PrintStatsInfo();

        // Initialize cam transform from the GameObject Transform component.
        CamTransform = this.GetComponent<Transform>();
        //Debug.Log(CamTransform.localPosition);

        // Initialize DirectionLight as the GameObject "Directional Light" by using Find method.
        DirectionLight = GameObject.Find("Directional Light");

        // Initialize LightTransform from the Transform component attached to DirectionLight using GetComponent method.
        LightTransform = DirectionLight.GetComponent<Transform>();
        //Debug.Log(LightTransform.localPosition + "Oy!");


    }

    public void RollDice()
    {
        switch(DiceRoll)
        {
            // An example of fall through break casing. 7 acts as 15. 
            case 7:
            case 15:
                Debug.Log("Mediocre damage, not bad");
                break;
            case 20:
                Debug.Log("Critical hit, the creature goes down!");
                break;
            default:
                Debug.Log("You completely missed and fell on your face.");
                break;
        }
    }

    /// <summary>
    ///Itirates through a collections list and displays the various party members.
    /// </summary>
    public void FindPartyMembers()
    {
        // Creating a collections list to hold party members.
        List<string> QuestPartyMembers = new()
        {
            "Grim the Barbarian",
            "Merlin the Wise",
            "Sterling the Knight"
        };

        QuestPartyMembers.Add("Arcturus the Mage");
        QuestPartyMembers.Insert(1, "Todd the Bard");

        for(int i = 0; i < QuestPartyMembers.Count; i++)
        {

            Debug.LogFormat("Index {0} : {1}", i, QuestPartyMembers[i]);

            if (QuestPartyMembers[i] == "Arcturus the Mage")
            {
                Debug.Log("The flows of magic are whimsical today");
            }
        }

        foreach(string partyMember in QuestPartyMembers)
        {
            Debug.LogFormat("{0} is in the party.", partyMember);
        }
    }

    /// <summary>
    /// Displays the current store inventory and determines pricing.
    /// </summary>
    public void DisplayInventory()
    {
        // Creating a dictionary collection to hold items.
        Dictionary<string, int> ItemInventory = new Dictionary<string, int>()
        {
            {"Potion", 5 },
            {"Antidote", 7 },
            {"Aspirin", 1 },
            {"Sword of the magi", 1000},
            {"Lifesblood of the broken", 55},
            {"Tears of a Hyacinth", 79},
            {"Helm of the Goliath", 3792}
        };

        // And logging said items using foreach loop.
        foreach (KeyValuePair<string, int> kvp in ItemInventory)
        {
            Debug.LogFormat("Item: {0} - {1}g", kvp.Key, kvp.Value);

            // if loop determines if character can afford item.
            if (kvp.Value <= CharacterGold)
            {
                Debug.LogFormat("You can afford the {0}", kvp.Key);
            }
            else
            {
                Debug.LogFormat("You can't afford {0}", kvp.Key);
            }
        }
    }

    /// <summary>
    /// Displays player lives and decrements accordingly. 
    /// </summary>
    public void HealthStatus()
    {
        while(PlayerLives > 0)
        {
            Debug.LogFormat("Current Lives: {0}", PlayerLives);
            PlayerLives--;
        }
        // When lives are at 0
        Debug.Log("Player KO");
    }

    void Update()
    {

    }

}