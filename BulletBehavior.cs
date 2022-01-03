using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : Destroyable<BulletBehavior>
    /* BulletBehavior is a subclass of destroyable.
    // THe generic type of Destroyable here is BulletBehavior.
    // That works because BulletBehavior inherits from Destroyable which
    is itself a child of MonoBehavior.
    */
{
    // Class inherits from destroyable, 
    public BulletBehavior()
    // Constructor sets Delay to 3f upon instantiation. 
    {
        this.OnscreenDelay = 3f;
    }
}
