using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string name;
    public int exp = 0;

    // Basic constructor example.
    public Character()
    {
        Reset();
    }

    // Constructor overloading example.
    public Character(string name)
    {
        this.name = name;
    }

    public virtual void PrintStatsInfo()
    {
        Debug.LogFormat("Hero: {0} - {1} EXP", name, exp);
    }

    private void Reset()
    {
        name = "Not assigned";
        exp = 0; 
    }
}

public class Paladin: Character
{
    public Weapon weapon;

    // Calls the parent class constructor. 
    public Paladin(string name, Weapon weapon): base(name)
    {
        this.weapon = weapon;
    }

    public override void PrintStatsInfo()
    {
        Debug.LogFormat("Welcome {0} - take up the {1}!", name, weapon.name);
    }
}
