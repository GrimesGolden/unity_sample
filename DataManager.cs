using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // System input output has all classes and methods needed to work with the filesystem.
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public class DataManager : MonoBehaviour, IManager
{

    //--------------------------------------------BEGIN VARIABLES-----------------------------------------------------------------------------
    private string _state;
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }

    private string _dataPath; // holds location of the string storing location of the path file, the directory holding all persistent data.
    private string _textFile;
    private string _streamingTextFile;
    private string _xmlLevelProgress;
    private string _xmlWeapons;
    private string _jsonWeapons;

    private List<Weapon> weaponInventory = new List<Weapon>
    {
        new Weapon("Sword of Doom", 100),
        new Weapon("Butterfly knives", 25),
        new Weapon("Brass Knuckles", 15),

    };

    //--------------------------------------------END VARIABLES-----------------------------------------------------------------------------

    //--------------------------------------------START METHODS-----------------------------------------------------------------------------
    void Awake()
    {
        _dataPath = Application.persistentDataPath + "/Player_Data/";

        Debug.Log(_dataPath);
        // Note: dataPath is under the hidden /Users/Library directory.

        _textFile = _dataPath + "Save_Data.txt";
        _streamingTextFile = _dataPath + "Streaming_Save_Data.txt";
        _xmlLevelProgress = _dataPath + "Progress_Data.xml";
        _xmlWeapons = _dataPath + "WeaponInventory.xml";
        _jsonWeapons = _dataPath + "WeaponJSON.json";


    }

    public void FilesystemInfo()
    {

        // A path is the location of a directory (file).
        // This method prints some of the information about the file system. 
        Debug.LogFormat("Path separator character:{0}", Path.PathSeparator);

        Debug.LogFormat("Directory separator character: {0}", Path.DirectorySeparatorChar);

        Debug.LogFormat("Current directory: {0}", Directory.GetCurrentDirectory());

        Debug.LogFormat("Temporary path: {0}", Path.GetTempPath());
    }

    public void NewTextFile()
    {
        if (File.Exists(_textFile))
        {
            Debug.Log("File already exists...");
            return; // exists method
        }

        File.WriteAllText(_textFile, "<SAVE DATA>\n\n");

        Debug.Log("New file created!");
    }

    public void UpdateTextFile()
    {
        if(!File.Exists(_textFile))
        {
            Debug.Log("File doesn't exist");
            return;
        }

        File.AppendAllText(_textFile, $"Game started: {DateTime.Now}\n");

        Debug.Log("File updated successfully!");
    }

    public void ReadFromFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Debug.Log("File doesn't exist...");
            return;
        }

        Debug.Log(File.ReadAllText(filename));
    }

    public void DeleteFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Debug.Log("File doesn't exist or has already been deleted...");

            return;
        }

        File.Delete(_textFile);
        Debug.Log("File successfully deleted!");
    }

    public void NewDirectory()
    {
        if(Directory.Exists(_dataPath))
        {
            Debug.Log("Directory already exists...");
            return;
            // Exists method without going any further.
        }

        Directory.CreateDirectory(_dataPath);
        Debug.Log("New directory created!");
    }

    public void DeleteDirectory()
    {
        if (!Directory.Exists(_dataPath))
        {
            Debug.Log("Directory doesn't exist or has already been deleted...");
            return;
        }

            //Just deletes the _dataPath directory, which was an empty folder to begin with!
            Directory.Delete(_dataPath, true);
            Debug.Log("Directory succesfully deleted!");
    }

    public void WriteToStream(string filename) // Use streams when dealing with big data files or complex objects. 
    {
        // Check if file doesn't exist.
        if (!File.Exists(filename))
        {
            // If file hasn't been created yet
            // Add a new StreamWriter instance called new Stream which
            // Uses the CreateText() method to create and open new file.
            StreamWriter newStream = File.CreateText(filename);
            // Then add header, close stream and print a debug message...

            newStream.WriteLine("<Save Data> for HERO BORN \n\n");
            newStream.Close();
            Debug.Log("New file created with StreamWriter!");
        }

        // If file already exists, just create new StreamWriter instance...
        StreamWriter writer = File.AppendText(filename);
        
        writer.WriteLine("Game ended: " + DateTime.Now);
        writer.Close();
        Debug.Log("File contents updated with StreamWriter!");
        
        // Write new line with current time, then close stream and print debug.
        // File is appending twice still. If this happens again, make a stack overflow. 
    }

    public void ReadFromStream(string filename)
    {
        // If file doesn't exist exit method, nothing to read.
        if (!File.Exists(filename))
        {
            Debug.Log("File doesn't exist...");
            return;
        }
        // If file does exist create new StreamReader instance...
        // Access file and print out entire contents using ReadToEnd()

       
        StreamReader streamReader = new StreamReader(filename);
        Debug.Log(streamReader.ReadToEnd());
        streamReader.Close(); // Chapter 12 didn't close the streamReader. This causes an IO sharing error.       
    }

    public void WriteToXML(string filename)
    {
        if(!File.Exists(filename))
        {
            // Create a new file stream
            FileStream xmlStream = File.Create(filename);

            // Then pass this file stream to an XmlWriter, a 'wrapper' to format xml.
            XmlWriter xmlWriter = XmlWriter.Create(xmlStream);

            // Specify xml version 1.0 with WriteStartDocument (creates XML Header)
            xmlWriter.WriteStartDocument();

            //Add the opening root element
            xmlWriter.WriteStartElement("level_progress");

            // Add individual elements to document using WriteElementString
            for (int i = 1; i < 5; i++)
            {
                xmlWriter.WriteElementString("level", "Level-" + i);
            }

            // Close the document using WriteEndElement (add closing level tag)
            xmlWriter.WriteEndElement();

            // Close Writer and Stream to release stream resources. 
            xmlWriter.Close();
            xmlStream.Close();
        }
    }

    public void SerializeXML()
    {
        // Create a new XmlSerializer instance. It's type is a list of weapons.
        var xmlSerializer = new XmlSerializer(typeof(List<Weapon>));

        // using this a new file stream under the _xmlWeapons file. 
        using(FileStream stream = File.Create(_xmlWeapons))
        {
            // Serialize the _xmlWeapons list.
            // Think of serialization as translating from one state to another
            // In this case from a List to .xml
            xmlSerializer.Serialize(stream, weaponInventory);
        }
    }

    public void DeserializeXML()
    {
        if (File.Exists(_xmlWeapons))
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Weapon>));

            using (FileStream stream = File.OpenRead(_xmlWeapons))
            {
                var weapons = (List<Weapon>)xmlSerializer.Deserialize(stream);

                foreach (var weapon in weapons)
                {
                    Debug.LogFormat("Weapon: {0} - Damage: {1}",
                        weapon.name, weapon.damage);
                }
            }
        }
    }

    public void SerializeJSON()
    {
        // Create new instance of shop
        WeaponShop shop = new WeaponShop();

        // Shop class has an inventory which is a Weapon<List> type.
        shop.inventory = weaponInventory;

        // Convert shop into a Json formatted string
        string jsonString = JsonUtility.ToJson(shop, true);

        // List wont work with json serialization unless it's first part of a class.
        using(StreamWriter stream = File.CreateText(_jsonWeapons))
        {
            stream.WriteLine(jsonString);
        }
    }

    public void DeserializeJSON()
    {
        if(File.Exists(_jsonWeapons))
        {
            using (StreamReader stream = new StreamReader(_jsonWeapons))
            {
                var jsonString = stream.ReadToEnd();

                // Specify we want to convert the json file into a <WeaponShop> object from jsonString. 
                var weaponData = JsonUtility.FromJson<WeaponShop>(jsonString);

                foreach (var weapon in weaponData.inventory)
                {
                    Debug.LogFormat("Weapon: {0} - Damage: {1}",
                        weapon.name, weapon.damage);
                }
            }
        }
    }
    //--------------------------------------------END METHODS-----------------------------------------------------------------------------

    //--------------------------------------------MAIN METHODS-----------------------------------------------------------------------------

    public void Initialize()
    {
        _state = "Data Manager initialized...";
        Debug.Log(_state);

        NewDirectory();
        SerializeXML();
        //DeserializeXML();
        SerializeJSON();
        //DeserializeJSON();
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    //--------------------------------------------END PROGRAM-----------------------------------------------------------------------------
}

/*
 * Utilizes the IManager interface.
 * Then utilizes the Initialize method to return the State
 * as a custom string.
 * */
