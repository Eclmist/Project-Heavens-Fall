using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
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
            GetActiveData();
            if (activeData == null) Debug.LogWarning("<color=#FF0000>No data found to load</color>");
        }
    }

    public void OnDrawGizmos()
    {
        if (activeData != null)
        {
            foreach (var activeDataDeathPoint in activeData.deathPoints)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(activeDataDeathPoint, 0.3f);
            }

            foreach (var trail in activeData.locationTrails)
            {
                if (trail == null) continue;

                for (int i = 1; i < trail.Count; i++)
                {
                    Debug.DrawLine(trail[i], trail[i-1]);
                }
            }
        }
    }

    public static void AddDeathEntry(Vector3 deathPoint)
    {
        string currentLevel = SceneManager.GetActiveScene().name;

        if (activeData == null || activeData.levelID != currentLevel)
        {
            GetActiveData();
        }

        activeData.deathPoints.Add(deathPoint);
    }

    public static void ReportLocation(Vector3 position)
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        if (activeData == null || activeData.levelID != currentLevel)
        {
            GetActiveData();
        }

        if (activeData.activeLocationTrail == null)
        {
            activeData.activeLocationTrail = new List<Vector3>();
        }

        activeData.activeLocationTrail.Add(position);
    }

    public static void TerminateLocation()
    {
        activeData.locationTrails.Add(activeData.activeLocationTrail);
        activeData.activeLocationTrail = null;
    }

    public static void GetActiveData()
    {
        string currentLevel = SceneManager.GetActiveScene().name;

        if (File.Exists(Application.persistentDataPath + "/ald/analyticsData.ald"))
            LoadData();

        //If Active Data is null
        foreach (var levelDeathData in data)
        {
            if (levelDeathData.levelID == currentLevel)
            {
                activeData = levelDeathData;
                return;
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
        if (!File.Exists(Application.persistentDataPath + "/ald/analyticsData.ald")) return false;

        FileStream file = new FileStream(
            Application.persistentDataPath + "/ald/analyticsData.ald", FileMode.Open);
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<LevelDeathData>));
            //BinaryFormatter bf = new BinaryFormatter();

            data = (List<LevelDeathData>)serializer.Deserialize(file);
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

        var directoryInfo = (new FileInfo(Application.persistentDataPath + "/ald/analyticsData.ald")).Directory;
        if (directoryInfo != null)
            directoryInfo.Create();

        FileStream file = new FileStream(
            Application.persistentDataPath + "/ald/analyticsData.ald", FileMode.OpenOrCreate);
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<LevelDeathData>));
            //BinaryFormatter bf = new BinaryFormatter();

            //print("Save" + currentProfile.names.Count + currentProfile.LevelsUnlocked);
            serializer.Serialize(file, data);
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
    [SerializeField] public List<List<Vector3>> locationTrails = new List<List<Vector3>>();
    [NonSerialized,XmlIgnore] public List<Vector3> activeLocationTrail;
}
