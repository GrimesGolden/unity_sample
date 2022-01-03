using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Declare a new generic class with a T type parameter.
// Recognized as generic due to it's T type parameter. 
public class Shop<T> where T : Collectable
    // Constrained to Collectable type
{
    // Add inventory list of type T to store any item types we initialize with.
    public List<T> inventory = new List<T>();

    // Uses the generic type from above to add an item to the list of generics.
    // But this method itself is non generic. 
    public void AddItem(T newItem)
    {
        inventory.Add(newItem);
    }

    // Create generic method with type U (represents any type)
    public int GetStockCount<U>() where U : T
        // Constrained U to equal whatever the initial generic type is.
    {
        // Declare a count.
        var stock = 0;

        foreach (var item in inventory)
            //Start a foreach loop
        {
            if (item is U)
                // If any item has the same type as the generic method calling type.
            {
                stock++;
                // Update count.
            }
        }

        // Return count.
        return stock;
    }
}
