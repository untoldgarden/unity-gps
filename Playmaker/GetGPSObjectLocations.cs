using UnityEngine;

namespace HutongGames.PlayMaker.Actions.UntoldGarden
{

	[ActionCategory("Untold Garden/GPS")]
	public class GetGPSObjectLocations : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Mesh Id of the objects.")]
		public FsmString objectName;

		[Tooltip("Freeze the average calculation of these objects from now. Saves processing power.")]
		public FsmBool freezeAverage;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Vector3 array variable to save locations to.")]
		public FsmArray locationsVariable;

		public override void Reset()
		{
			objectName = null;
			freezeAverage = false;
			locationsVariable = null;
		}

		// Code that runs on entering the state.
		public override void OnEnter()
		{
			locationsVariable = new FsmArray();

			foreach (Vector3 location in GPSManager.MainManager.GetLocations(objectName.Value, freezeAverage.Value))
			{
				locationsVariable.Resize(locationsVariable.Length + 1);
				locationsVariable.Set(locationsVariable.Length - 1, location);
			}
			Finish();
		}


	}

}
