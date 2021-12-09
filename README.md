# unity-gps

Implementation of AR + GPS in Unity. This was done in a haste, is not optimized, and is not very cleanly done. 
GPS seem to be not very accurate, it might be due to issues in our implementation, to issues in AR + GPS, or to other issues.

# Dependencies
* AR + GPS Asset https://assetstore.unity.com/packages/tools/integration/ar-gps-location-134882
* Playmaker https://assetstore.unity.com/packages/tools/visual-scripting/playmaker-368 (Not crucial, if used without playmaker just delete folder PlayMaker.)

How to use:
Place GPSManager and AR + GPS WebMapLoader on a GameObject in Scene.
Trigger GPSManager.InitFromXML at start with:
* A xml of locations obtained from https://editor.unity-ar-gps-location.com/
* A prefabdatabase of prefabs, one for each location. Make sure the MeshID corresponds to the location name in the xml

Since GPS isn't very accurate, and jumps around a lot depending on local circumstances, we use the average of the past n gps locations to determine the location we place our prefab on. 
GPS gets more accurate over time, so let it run for a while before getting the average. 

You get the average of a location by calling GPSManager.GetFirstLocation with the name of the location (MeshID) which returns a Vector.
