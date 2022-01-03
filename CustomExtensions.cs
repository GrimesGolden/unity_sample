using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Declare a namespace to hold all extensions classes/methods
namespace CustomExtensions
{
    // Declare a static class named StringExtensions for organizational purposes.
    public static class StringExtensions
    {
        // Add a static method to this class, first param marks it as extension.
        public static void FancyDebug(this string str)
        {
            //Prints out a debug message whenever FancyDebug is executed, as.
            // Example: exampleString.FancyDebug();
            Debug.LogFormat("This string contains {0} characters.", str.Length);
        }
    }
}
