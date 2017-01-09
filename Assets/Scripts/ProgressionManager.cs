using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;



public class ProgressionManager : MonoBehaviour
{
    private static PlayerProgression currentProfile;

    public static PlayerProgression CurrentProfile
    {
        get { return currentProfile; }
    }

    public static void CreateNewProfile(string profileName = "DefaultProfile")
    {
        currentProfile = new PlayerProgression(profileName);
    }

    public static string[] GetSavedProfiles()
    {
        string[] savefiles = System.IO.Directory.GetFiles(Application.persistentDataPath, "*.save");
        for (int i = 0; i < savefiles.Length; i++)
        {
            savefiles[i] = savefiles[i].Substring(Application.persistentDataPath.Length + 1);
            savefiles[i] = savefiles[i].Substring(0, savefiles[i].Length - 5);
        }

        return savefiles;
    }

    public static bool LoadProfile(string profileName = "DefaultProfile")
    {
        if (!File.Exists(Application.persistentDataPath + "/" + currentProfile.profileName + ".save")) return false;

        FileStream file = new FileStream(
            Application.persistentDataPath + "/" + profileName + ".save", FileMode.Open);
        try
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(PlayerProgression));
            BinaryFormatter bf = new BinaryFormatter();

            currentProfile = (PlayerProgression) bf.Deserialize(file);
            currentProfile.ReconstructDictionary();
            //print("Load"+currentProfile.names.Count + currentProfile.LevelsUnlocked);
            file.Close();
            return true;
        }
        catch (Exception e)
        {
            print(e);
            return false;
        }
        finally
        {
            file.Close();
        }
    }

    public static bool SaveProfile()
    {
        FileStream file = new FileStream(
            Application.persistentDataPath + "/" + currentProfile.profileName + ".save", FileMode.OpenOrCreate);
        try
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(PlayerProgression));
            BinaryFormatter bf = new BinaryFormatter();

            //print("Save" + currentProfile.names.Count + currentProfile.LevelsUnlocked);
            bf.Serialize(file, currentProfile);
            file.Close();
            return true;
        }
        catch (Exception e)
        {
            print(e);
            return false;
        }
        finally
        {
            file.Close();
        }
    }

    public static bool DeleteProfile(string profileName)
    {
        if (!File.Exists(Application.persistentDataPath + "/" + profileName + ".save")) return false;
        File.Delete(Application.persistentDataPath + "/" + profileName + ".save");
        return true;
    }
}

[Serializable]
public class PlayerProgression
{
    //public string LevelsUnlocked = "";
    public string profileName;
    //public List<string> names = new List<string>();
    public List<StringLevelPair> StringLevelPairs = new List<StringLevelPair>();
    [NonSerialized]private Dictionary<string,Level> PlayedLevels = new Dictionary<string, Level>();

    [Serializable]
    public struct StringLevelPair
    {
        public string str;
        public Level lvl;

        public StringLevelPair(string str, Level lvl)
        {
            this.str = str;
            this.lvl = lvl;
        }
    }

    [Serializable]
    public class Level
    {
        public bool cleared;
        public int checkpointIndex;
        //public List<bool> levelLights;

        public Level()
        {
            //TODO: Add accessors for this list
            //levelLights = new List<bool>();
            this.checkpointIndex = 0;
            this.cleared = false;
        }
    }

    public PlayerProgression()
    {
        //DONT CALL THIS
    }

    public PlayerProgression(string profileName)
    {
        this.profileName = profileName;
    }

    public bool IsLevelCleared(string levelId)
    {
        //return names.Contains(levelId);
        if (PlayedLevels.ContainsKey(levelId)) return PlayedLevels[levelId].cleared;
        return false;
    }

    public bool IsLevelUnlocked(string levelId)
    {
        return PlayedLevels.ContainsKey(levelId);
    }

    public void SetLevelUnlocked(string levelId)
    {
        if (!PlayedLevels.ContainsKey(levelId)) AddNewLevelEntry(levelId, new Level());
    }

    public void SetLevelCleared(string levelId)
    {
        //AddNewLevelEntry(levelId,new Level());

        Assert.SoftAssert(PlayedLevels.ContainsKey(levelId), "You shouldnt be clearing a level that you havent played yet. No Action Taken");
        if (PlayedLevels.ContainsKey(levelId)) PlayedLevels[levelId].cleared = true;
    }

    
    public int GetCheckpointIndex(string levelId)
    {
        Assert.SoftAssert(PlayedLevels.ContainsKey(levelId), "You shouldnt be trying to get the checkpoint of a level you havent played." + StringLevelPairs.Count);
        if (PlayedLevels.ContainsKey(levelId)) return PlayedLevels[levelId].checkpointIndex;
        return -1;
    }
    
    public void SetLevelCheckpoint(string levelId, int checkpointIndex)
    {
        if (!PlayedLevels.ContainsKey(levelId)) AddNewLevelEntry(levelId, new Level());
        PlayedLevels[levelId].checkpointIndex = checkpointIndex;
    }

    //For Debug Purposes
    public void OverrideLevelCleared(string levelId)
    {
        //AddNewLevelEntry(levelId, new Level());


        Debug.LogWarning("<color=#FF0000><b>Overriding Level cleared status. Creating entry if it doesnt exist</b></color>" + StringLevelPairs.Count);
        if (!PlayedLevels.ContainsKey(levelId)) AddNewLevelEntry(levelId, new Level());
        PlayedLevels[levelId].cleared = true;
    }

    private void AddNewLevelEntry(string str, Level lvl)
    {
        StringLevelPairs.Add(new StringLevelPair(str, lvl));
        //LevelsUnlocked += "$" + str;
        //names.Add(str);
        PlayedLevels.Add(str, lvl);
    }

    public void ReconstructDictionary()
    {
        //char[] delimiters = {'$'};
        //string[] levels = LevelsUnlocked.Split(delimiters,StringSplitOptions.RemoveEmptyEntries);
        //names.AddRange(levels);

        PlayedLevels = new Dictionary<string, Level>();
        foreach (var stringLevelPair in StringLevelPairs)
        {
            PlayedLevels.Add(stringLevelPair.str, stringLevelPair.lvl);
        }
    }
}

