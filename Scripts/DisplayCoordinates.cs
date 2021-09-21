using ARLocation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCoordinates : MonoBehaviour
{
    public Text text;
    public ARLocationProvider locProvider;
    public static Transform positionTransform;

    // Update is called once per frame
    void Update()
    {
        text.text = "La: " + locProvider.CurrentLocation.latitude;
        text.text += "\nLo: " + locProvider.CurrentLocation.longitude;

        if (positionTransform != null)
        {
            text.text += "\nX: " + positionTransform.position.x;
            text.text += "\nY: " + positionTransform.position.y;
            text.text += "\nZ: " + positionTransform.position.z;
        }
    }
}
