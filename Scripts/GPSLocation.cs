using ARLocation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GPSLocation : MonoBehaviour
{
    private PlaceAtLocation placeAtComponent;
    /// <summary>
    /// Our average position
    /// Used since the GPS positions are jumping around a lot
    /// </summary>
    public Vector3 AveragePosition { get; private set; }

    /// <summary>
    /// The amount of positions we use to calculate the average
    /// </summary>
    public int averagePositionsNumber = 20;

    private List<Vector3> positions = new List<Vector3>();

    /// <summary>
    /// Register this object with GPSManager
    /// </summary>
    private void Start()
    {
        // Register with GPS Manager
        GameObject.FindGameObjectWithTag("GPSManager").GetComponent<GPSManager>().RegisterGPSObject(this);
    }

    /// <summary>
    /// TODO: Restructure this to avoid endlessly trying to add PLaceAtLocation
    /// </summary>
    void Update()
    {
        if (placeAtComponent == null)
        {
            placeAtComponent = GetComponent<PlaceAtLocation>();
            if (placeAtComponent == null) return;

            placeAtComponent.ObjectPositionUpdated.AddListener(Relocated);
        }
    }

    /// <summary>
    /// Listens to the ObjectPositionUpdated event from PlaceAtLocation
    /// Calculates an average of the past 20 positions
    /// </summary>
    /// <param name="go">Location prefab gameobject</param>
    /// <param name="loc">Location</param>
    /// <param name="numUpdates"></param>
    private void Relocated(GameObject go, Location loc, int numUpdates)
    {
        if (AveragePosition == null) return;
        if (transform.position == Vector3.zero) return;

        positions.Add(transform.position);

        if (positions.Count > averagePositionsNumber) positions.RemoveAt(0);

        AveragePosition = positions.Aggregate(Vector3.zero, (acc, v) => acc + v) / positions.Count;

        /*
        if (updates > 50) updates = 50;
        AveragePosition = new Vector3(AveragePosition.x * updates, AveragePosition.y * updates, AveragePosition.z * updates);
        AveragePosition += transform.position;
        updates++;
        AveragePosition = new Vector3(AveragePosition.x / updates, AveragePosition.y / updates, AveragePosition.z / updates);
        */
    }

    /// <summary>
    /// Stops position average calculation
    /// </summary>
    public void FreezeAverage()
    {
        placeAtComponent.ObjectPositionUpdated.RemoveListener(Relocated);
    }

    /// <summary>
    /// Starts position average calculation
    /// </summary>
    public void UnfreezeAverage()
    {
        placeAtComponent.ObjectPositionUpdated.AddListener(Relocated);
    }

}
