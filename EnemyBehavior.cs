using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // Declare variable to store the Player GameObject location.
    public Transform Player;

    // Declare a variable to store PatrolRoute empty parent GameObject.
    public Transform PatrolRoute;

    // Declare a list variable to hold all the child Transform components.
    public List<Transform> Locations;

    // Represents the current location enemy is walking towards.
    private int _locationIndex = 0;

    // Declare a NavMeshAgent.
    private NavMeshAgent _agent;

    // Private int to represent enemy HP with public get/set. 
    private int _lives = 3;
    public int EnemyLives
    {
        get { return _lives;  }
        // Only this class can set itself, lives is essentially read only. 
        private set
        {
            _lives = value;

            // If lives are set below 0 then destroy this gameObject.
            if(_lives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy down.");
            }
        }
    }

    void Start()
    {
        // The _agent is assigned to the Enemy GameObjects NavMeshAgent.
        _agent = GetComponent<NavMeshAgent>();

        // Assign variable to Player objects current Transform position.
        Player = GameObject.Find("Player").transform;

        // Use start to call this method when game begins.
        InitializePatrolRoute();

        // Call this method after initializing Locations list.
        MoveToNextPatrolLocation();

    }

    void Update()
    {
        if(_agent.remainingDistance < 0.2f && !_agent.pathPending)
        {
            // If there is less than 0.2 from destination.
            // And the path is not pending.
            // Then execute next destination method.
            MoveToNextPatrolLocation();
        }
    }

    // Private utlity method to procedurally fill Locations with Transform values.
    void InitializePatrolRoute()
    {
        // Loop through each child GameObject in PatrolRoute.
        foreach(Transform child in PatrolRoute)
        {
            // Add sequential child Transform component to list of locations.
            Locations.Add(child);
        }
    }

    void MoveToNextPatrolLocation()
    {
        // destination is a Vector3 position in 3D space.
        // position referenced the Transform components vector3 position.

        // Ensure locations list is populated before continuing.
        if (Locations.Count == 0)
            return;

        _agent.destination = Locations[_locationIndex].position;

        // The modulo of any integer < than x is == x.
        // But modulo when x == x is 0.
        // Thus when we hit the final item in the list this formula returns 0.
        // Resetting count. 
        _locationIndex = (_locationIndex + 1) % Locations.Count;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            // If collision with player, change destination to Players position.
            _agent.destination = Player.position;

            Debug.Log("Player detected - attack!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.name == "Player")
        {
            Debug.Log("Player out of range, resume patrol");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If the enemy collides with a bullet, reduce lives by 1. 
        if(collision.gameObject.name == "Bullet(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Critical hit!");
        }
    }
}
