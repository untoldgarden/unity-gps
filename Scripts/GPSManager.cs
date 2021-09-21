using ARLocation;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static ARLocation.PrefabDatabase;

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

    public void InitFromXML(PrefabDatabase PrefabDatabase, TextAsset XmlDataFile)
    {
        UntoldGarden.AR.Logger.Log("GPS Manager InitFromXML");
        loader.PrefabDatabase = PrefabDatabase;
        loader.XmlDataFile = XmlDataFile;
        loader.enabled = true;

        MainManager = this;
    }

    public void RegisterGPSObject(GPSLocation location)
    {
        UntoldGarden.AR.Logger.Log("GPS Manager RegisterGPSObject " + location.name);
        foreach (PrefabDatabaseEntry entry in loader.PrefabDatabase.Entries)
        {
            if (location.gameObject.name == string.Format("{0}(Clone)", entry.Prefab.name))
            {
                GPSObjects.Add(new GPSObject(location, entry.MeshId));
                return;
            };
        }
    }

    public List<Vector3> GetLocations(string meshId, bool freeze = false, bool onlyFirst = false)
    {
        UntoldGarden.AR.Logger.Log("GPS Manager GetLocations " + meshId);
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

    public Vector3 GetFirstLocation(string meshId, bool freeze = false)
    {
        UntoldGarden.AR.Logger.Log("GPS Manager get first location");
        List<Vector3> locations = GetLocations(meshId, freeze, true);
        if (locations.Count > 0) return locations[0];
        else return Vector3.zero;
    }

}
