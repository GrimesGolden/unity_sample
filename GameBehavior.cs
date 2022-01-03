using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This namespace allows access to the Text variable type. 
using UnityEngine.UI;
// Add SceneManagement namespace, this handles scene-related logic like loading scenes.
using UnityEngine.SceneManagement;
using CustomExtensions;

public class GameBehavior : MonoBehaviour, IManager
{   
    public int MaxItems = 5;

    public Stack<string> LootStack = new Stack<string>();

    // Text variables which are connected in the inspector panel. 
    public Text HealthText;
    public Text ItemText;
    public Text ProgressText;

    // Creating a UI button to connect win condition.
    public Button WinButton;
    // And one to connect loss condition.
    public Button LossButton;

    // Declare a new variable of type PlayerBehavior.
    public PlayerBehavior playerBehavior; 

    public void UpdateScene(string updatedText)
    {
        // Updates progress text and freezes gameplay.
        ProgressText.text = updatedText;
        Time.timeScale = 0f;
    }

    // Private variables to store items collected and total player lives.
    private int _itemsCollected = 0;
    // Declare a public variable items with get and set properties.
    public int Items
    {
        // Return the value in _itemsCollected whenever Items is accessed from an outside class.
        get { return _itemsCollected; }

        // When items is updated, update private value and print a debug statement.
        set
        {
            _itemsCollected = value;

            // Every time an item is collected, update the text property of ItemText to show updated items count.
            ItemText.text = "Items Collected: " + Items;

            // If player has collected more or equal to MaxItems, they have won. 
            if (_itemsCollected >= MaxItems)
            {
                WinButton.gameObject.SetActive(true);
                UpdateScene("You've found all the items!");       
            }
            else
            {
                // Else we have not won so simply update the UI
                ProgressText.text = "Item found, only " +
                    (MaxItems - _itemsCollected) + " more to go!";
            }
        }
    }

    private int _playerHP = 10;
    // Do the same for HP
    public int HP
    {
        // Upon get simply return players hp. 
        get { return _playerHP; }
        set
        // Yet on set update the value of private variable as well as text, also print a debug statement. 
        {
            _playerHP = value;

            // Every time players health is damaged, health text updates. 
            HealthText.text = "Player Health: " + HP;
            Debug.LogFormat("Lives: {0}", _playerHP);

            if(_playerHP <= 0)
            {
                LossButton.gameObject.SetActive(true);
                UpdateScene("Game Over!");      
            }
            else
            {
                ProgressText.text = "Ouch... that's got to hurt";
            }
        }
    }

    private string _state;

    public string State
    {
        get { return _state; }
        set { _state = value;  }
    }

    // Declare a public delegate type to hold a method that accepts a string.
    public delegate void DebugDelegate(string newText);

    // Create a new DebugDelegate instance and assign it a matching method.
    public DebugDelegate debug = Print;

    //----------------------------------------------END VARIABLES----------------

    // Set initial values for Items and Health
    void Start()
    {
        ItemText.text += _itemsCollected;
        HealthText.text += _playerHP;

        

        Initialize();
    }

    public void Initialize()
    {
        // Initialize a new Shop generic class.
        // but we set the generic type as a string.
        // Think of generics as Eevee from pokemon, it can be many types (until declared or evolved)
        var itemShop = new Shop<Collectable>();

        // Add two strings, because that's the type of itemShop.
        // But it could have been any type when initialized. 
        itemShop.AddItem(new Potion());
        itemShop.AddItem(new Antidote());

        // itemShop only allows collectables, in this case GetStockCount is checking for Potion subclass specifically. 
        Debug.Log("There are " + itemShop.GetStockCount<Potion>() + "" +
            " items for sale.");

        debug(_state);
    }

    public void RestartScene()
    {
        //Call from within static Utilities class, the RestartLevel method.
        Utilities.RestartLevel(0);
    }

    public void PrintLootReport()
    {
        // This method gets called in ItemBehavior when an item is collected.

        // Pops first item from stack and then displays next item in stack using peek. 
        var currentItem = LootStack.Pop();
        var nextItem = LootStack.Peek();

        Debug.LogFormat("Collected {0}, next item to collect: {1}", currentItem, nextItem);
        Debug.LogFormat("There are {0} random loot items waiting for you!", LootStack.Count);
    }

    public static void Print(string newText)
        // Matching method for delegate prints a debug log.
    {
        Debug.Log(newText);
        // Debug has been effectively delegated to "debug" delegate variable.
    }

    void OnEnable()
        // OnEnable is called whenever object the script is attached to becomes active in scene.
    {

        GameObject player = GameObject.Find("Player");

        // assign playerBehavior to the PlayerBehavior script on player object.
        playerBehavior = player.GetComponent<PlayerBehavior>();

        playerBehavior.playerJump += HandlePlayerJump;

        // Subscribe the playerBehavior to the playerJump event using +=
        // When playerJump executes perform HandlePlayerJump method.
        debug("Jump event subscribed...");
    }

    void OnDisable()
        // Companion to OnEnable, called when object becomes inactive.
    {
        playerBehavior.playerJump -= HandlePlayerJump;
        debug("Jump event unsubscribed...");
    }

    public void HandlePlayerJump()
        // Prints a message using delegated Debug. 
    {
        debug("Player has jumped...");
    }

}

// Both private variables are now readable. but only through their public counterparts.
// They can only be changed through GameBehavior. 
