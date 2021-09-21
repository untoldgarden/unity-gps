using ARLocation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AveragePlacer : MonoBehaviour
{
    public GPSLocation locScript;
    public GameObject averagePrefab;
    private GameObject instantiatedPrefab;
    private bool firstMove = false;

    private Vector3 lastPosition;
    private PlaceAtLocation placeAtComponent;

    private void Start()
    {
        placeAtComponent = GetComponent<PlaceAtLocation>();
        placeAtComponent.ObjectPositionUpdated.AddListener(Updated);
    }

    private void Updated(GameObject arg0, Location arg1, int arg2)
    {
        placeAtComponent.ObjectPositionUpdated.RemoveListener(Updated);

        firstMove = true;
        instantiatedPrefab = Instantiate(averagePrefab, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (locScript == null) return;

        if (firstMove)
        {
            instantiatedPrefab.transform.position = Vector3.MoveTowards(instantiatedPrefab.transform.position, locScript.AveragePosition, Time.deltaTime * 1f);
        }
    }
}
