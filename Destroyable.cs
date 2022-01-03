using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable<T> : MonoBehaviour where T : MonoBehaviour
    // Destroyable is constrained to only accept types of MonoBehavior class.
{

    // FLoat to hold time before destruction of object.
    public float OnscreenDelay;
  
    void Start()
    // On start destroy object after specified delay.
    // Will destroy instantly if no value given. 
    {
        Destroy(this.gameObject, OnscreenDelay);
    }

  
}
