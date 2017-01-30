using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class AnalyticsManager : MonoBehaviour
{
    private static List<LevelDeathData> data = new List<LevelDeathData>();
    private static LevelDeathData activeData;

    [Header("Buttons")] public bool LoadLevelAnalyticsData;

    public void Update()
    {
        if (LoadLevelAnalyticsData)
        {
            LoadLevelAnalyticsData = false;
            LoadData();
            if (activeData == null) Debug.LogWarning("<color=#FF0000>No data found to load</color>");
        }
    }

    public void OnDrawGizmos()
    {
        if (activeData != null)
        {
            foreach (var activeDataDeathPoint in activeData.deathPoints)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(activeDataDeathPoint, 1);
            }
        }
    }

    public static void AddEntry(Vector3 deathPoint)
    {
        string currentLevel = SceneManager.GetActiveScene().name;

        if (activeData == null || activeData.levelID != currentLevel)
        {
            GetActiveData();
        }

        activeData.deathPoints.Add(deathPoint);
    }

    public static void GetActiveData()
    {
        string currentLevel = SceneManager.GetActiveScene().name;

        //If Active Data is null
        foreach (var levelDeathData in data)
        {
            if (levelDeathData.levelID == currentLevel)
            {
                activeData = levelDeathData;
            }
        }

        //If no active data found
        activeData = new LevelDeathData();
        activeData.levelID = currentLevel;
        data.Add(activeData);
    }

    //Serialize data
    public static bool LoadData(string profileName = "DefaultProfile")
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        print(currentLevel);
        if (!File.Exists(Application.persistentDataPath + "/ald/analyticsData_" + currentLevel + ".ald")) return false;

        FileStream file = new FileStream(
            Application.persistentDataPath + "/ald/analyticsData_" + currentLevel + ".ald", FileMode.Open);
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LevelDeathData));
            //BinaryFormatter bf = new BinaryFormatter();

            activeData = (LevelDeathData)serializer.Deserialize(file);
            //currentProfile.ReconstructDictionary();
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


    //Deserialize data
    public static bool SaveData()
    {
        if (activeData == null)
        {
            Debug.LogWarning("<color=#FF0000>No Data found to serialize</color>");
            return false;
        }

        (new FileInfo(Application.persistentDataPath + "/ald/analyticsData_" + activeData.levelID + ".ald")).Directory
            .Create();

        FileStream file = new FileStream(
            Application.persistentDataPath + "/ald/analyticsData_" + activeData.levelID + ".ald", FileMode.OpenOrCreate);
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LevelDeathData));
            //BinaryFormatter bf = new BinaryFormatter();

            //print("Save" + currentProfile.names.Count + currentProfile.LevelsUnlocked);
            serializer.Serialize(file, activeData);
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

    
}

[Serializable, XmlRoot("LevelDeathData")]
public class LevelDeathData
{
    [SerializeField] public string levelID;
    [SerializeField] public List<Vector3> deathPoints = new List<Vector3>();
}
