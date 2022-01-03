using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Note that this is a structure and not a class. The key difference being
that it passes by value. Value meaning it creates its own copy in memory.
Opposed to a class that passes by reference creating a reference to a single unique location in memory.
*/

[System.Serializable]
public struct Weapon
{
    public string name;
    public int damage;

    public Weapon(string name, int damage)
    {
        this.name = name;
        this.damage = damage;
    }

    public void PrintWeaponStats()
    {
        Debug.LogFormat("Weapon: {0} - {1} DMG", name, damage);
    }
}

public class WeaponShop
{
    public List<Weapon> inventory;
}
