using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    // Stores a reference to the game behavior script.
    public GameBehavior GameManager;

    void Start()
    {
        // Initialize GameManager object (an instance of GameBehavior) by looking it up in the scene with Find()
        // Then adding a call to GetComponent()
        GameManager = GameObject.Find("Game_Manager").
                    GetComponent<GameBehavior>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Destroy(this.transform.gameObject);
            Debug.Log("Item collected!");

            // Increment the items property in the GameManager class
            // Note this happens after the object is destroyed.
            GameManager.Items += 1;

            GameManager.PrintLootReport();
        }
    }
}
