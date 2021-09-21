using System;
using UnityEngine;
using UntoldGarden;
using Logger = UntoldGarden.AR.Logger;

namespace HutongGames.PlayMaker.Actions.UntoldGarden
{

    [ActionCategory("Untold Garden/GPS")]
    public class GetGPSObjectLocation : RuntimeNavMeshBase
    {
        [RequiredField]
        [Tooltip("Mesh Id of the object.")]
        public FsmString objectName;

        [Tooltip("Freeze the average calculation of this object from now. Saves processing power.")]
        public FsmBool freezeAverage;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Vector3 variable to save location to.")]
        public FsmVector3 locationVariable;

        [Tooltip("Placeholder location for playing in editor.")]
        public FsmVector3 placeholderLocation;

        public FsmBool setYAtDefaultPlane = true;

        [Tooltip("Limits the position within a certain range of user origin, to avoid dangerous errors.")]
        public FsmFloat geofencingLimit;

        [UIHint(UIHint.Variable)]
        public FsmVector3 userOrigin;

        public override void Reset()
        {
            objectName = null;
            freezeAverage = false;
            locationVariable = null;
        }

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            base.OnEnter();

            Vector3 pos = new Vector3();
#if UNITY_EDITOR
            pos = placeholderLocation.Value;
            //Logger.Log("Getting editor placeholder location");
#else
			Logger.Log("Getting real gps location");
            try
            {
                pos = GPSManager.MainManager.GetFirstLocation(objectName.Value, freezeAverage.Value);
            }
            catch (Exception e)
            {
                Logger.Log("Error getting GPS position: " + e);
                pos = placeholderLocation.Value;
                Logger.Log("Set pos as default since it was null");
            }
#endif

            //Logger.Log("Got gps value");
            if (setYAtDefaultPlane.Value)
            {
                pos.y = runtimeNavMesh.GetDefaultPlane().transform.position.y;
            }
            //Logger.Log("Set Y");

            //Makes sure the point is within the geofencing limit of the origin
            if (userOrigin.Value != null && geofencingLimit.Value != 0 && Vector3.Distance(pos, userOrigin.Value) > geofencingLimit.Value)
                pos = userOrigin.Value.GetPointAtDistanceBetweenTwoPoints(pos, geofencingLimit.Value);

            //Logger.Log("Set geofencing");
            locationVariable.Value = pos;
            Logger.Log($"Location vector is {locationVariable.Value} and distance to origin is {Vector3.Distance(userOrigin.Value, pos)}");
            //Debug.Log($"Location vector is {locationVariable.Value} and distance to origin is {Vector3.Distance(userOrigin.Value, pos)}");
            Finish();
        }
    }
}