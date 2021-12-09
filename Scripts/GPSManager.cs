using ARLocation;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static ARLocation.PrefabDatabase;

/// <summary>
/// Managar for GPS locations
/// Stores a list of GPSObjects, each which is a prefab at a location
/// </summary>
public class GPSManager : Singleton<GPSManager>
{
    public static GPSManager MainManager;

    public WebMapLoader loader;
    private List<GPSObject> GPSObjects = new List<GPSObject>();


    public struct GPSObject
    {
        public GPSLocation location;
        public string meshId;

        public GPSObject(GPSLocation location, string meshId)
        {
            this.location = location;
            this.meshId = meshId;
        }
    }

    public void Awake()
    {
        loader.enabled = false;
    }

    /// <summary>
    /// Initialises GPS locations
    /// Run this at start for all locations since it takes a while before they normalise.
    /// Playmaker implementation triggers this from ImportGPSObjects.cs
    /// </summary>
    /// <param name="PrefabDatabase">Prefabs, one for each location</param>
    /// <param name="XmlDataFile">The file of locations, create this at https://docs.unity-ar-gps-location.com/map/ </param>
    public void InitFromXML(PrefabDatabase PrefabDatabase, TextAsset XmlDataFile)
    {
        //UntoldGarden.AR.Logger.Log("GPS Manager InitFromXML");
        loader.PrefabDatabase = PrefabDatabase;
        loader.XmlDataFile = XmlDataFile;
        loader.enabled = true;

        MainManager = this;
    }

    /// <summary>
    /// Triggered by a new GPSObject being added to the scene
    /// </summary>
    /// <param name="location"></param>
    public void RegisterGPSObject(GPSLocation location)
    {
        //UntoldGarden.AR.Logger.Log("GPS Manager RegisterGPSObject " + location.name);
        foreach (PrefabDatabaseEntry entry in loader.PrefabDatabase.Entries)
        {
            if (location.gameObject.name == string.Format("{0}(Clone)", entry.Prefab.name))
            {
                GPSObjects.Add(new GPSObject(location, entry.MeshId));
                return;
            };
        }
    }

    /// <summary>
    /// Get a list of locations
    /// TODO: I don't remember why a list would be useful?
    /// </summary>
    /// <param name="meshId">The location prefab we're getting</param>
    /// <param name="freeze">Stops calculation of average position</param>
    /// <param name="onlyFirst">Get only first location</param>
    /// <returns></returns>
    public List<Vector3> GetLocations(string meshId, bool freeze = false, bool onlyFirst = false)
    {
        List<Vector3> locations = new List<Vector3>();

        foreach(GPSObject obj in GPSObjects)
        {
            if (obj.meshId.ToLower().Trim().Equals(meshId.ToLower().Trim()))
            {
                locations.Add(obj.location.AveragePosition);
                if (freeze) obj.location.FreezeAverage();
                else obj.location.UnfreezeAverage();

                if (onlyFirst) return locations;
            }
        }

        return locations;
    }

    /// <summary>
    /// Get first saved average location
    /// Use this to get the location of a GPSObject
    /// </summary>
    /// <param name="meshId">The location prefab we're getting</param>
    /// <param name="freeze">Stops calculation of average position</param>
    /// <returns></returns>
    public Vector3 GetFirstLocation(string meshId, bool freeze = false)
    {
        //UntoldGarden.AR.Logger.Log("GPS Manager get first location");
        List<Vector3> locations = GetLocations(meshId, freeze, true);
        if (locations.Count > 0) return locations[0];
        else return Vector3.zero;
    }

}
