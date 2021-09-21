using ARLocation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GPSLocation : MonoBehaviour
{
    private PlaceAtLocation placeAtComponent;
    public Vector3 AveragePosition { get; private set; }

    private List<Vector3> positions = new List<Vector3>();

    private void Start()
    {
        // Register with GPS Manager
        GameObject.FindGameObjectWithTag("GPSManager").GetComponent<GPSManager>().RegisterGPSObject(this);
    }

    void Update()
    {
        if (placeAtComponent == null)
        {
            placeAtComponent = GetComponent<PlaceAtLocation>();
            if (placeAtComponent == null) return;

            placeAtComponent.ObjectPositionUpdated.AddListener(Relocated);
        }
    }

    private void Relocated(GameObject go, Location loc, int numUpdates)
    {
        if (AveragePosition == null) return;
        if (transform.position == Vector3.zero) return;

        positions.Add(transform.position);

        if (positions.Count > 20) positions.RemoveAt(0);

        AveragePosition = positions.Aggregate(Vector3.zero, (acc, v) => acc + v) / positions.Count;

        /*
        if (updates > 50) updates = 50;
        AveragePosition = new Vector3(AveragePosition.x * updates, AveragePosition.y * updates, AveragePosition.z * updates);
        AveragePosition += transform.position;
        updates++;
        AveragePosition = new Vector3(AveragePosition.x / updates, AveragePosition.y / updates, AveragePosition.z / updates);
        */
    }

    public void FreezeAverage()
    {
        placeAtComponent.ObjectPositionUpdated.RemoveListener(Relocated);
    }

    public void UnfreezeAverage()
    {
        placeAtComponent.ObjectPositionUpdated.AddListener(Relocated);
    }

}
