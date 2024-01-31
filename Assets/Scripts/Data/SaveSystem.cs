using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    //--------------------------------------------------------------------------
    public static void SaveGame()
    {
        // Used to save the general game data into a binary file
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedata.gamedata";
        FileStream stream = new FileStream(path, FileMode.Create);
        GeneralGameData data = new GeneralGameData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    //--------------------------------------------------------------------------
    public static GeneralGameData LoadGame()
    {
        // Used to load the general game data from the binary file into the game data class
        string path = Application.persistentDataPath + "/savedata.gamedata";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GeneralGameData data = formatter.Deserialize(stream) as GeneralGameData;
            stream.Close();

            return data;
        }
        else
        {
            // May need to add base-case / stopping condition
            SaveGame();
            return LoadGame();
        }
    }

    //--------------------------------------------------------------------------
    public static void SaveArmy()
    {
        // Uses binary formatter to seralize the data into the binary file
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedata.armydata";
        FileStream stream = new FileStream(path, FileMode.Create);

        ArmyGameData data = new ArmyGameData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    //--------------------------------------------------------------------------
    public static ArmyGameData LoadArmy()
    {
        // Loads the army data file
        string path = Application.persistentDataPath + "/savedata.armydata";
        if (File.Exists(path))
        {
            // Uses binary formatter to serialize the binary file into the data class
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ArmyGameData data = formatter.Deserialize(stream) as ArmyGameData;
            stream.Close();

            return data;
        }
        else
        {
            // May need to add base-case / stopping condition
            SaveArmy();
            return LoadArmy();
        }
    }
}