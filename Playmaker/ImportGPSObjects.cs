using ARLocation;
using UnityEngine;
using Logger = UntoldGarden.AR.Logger;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Untold Garden/GPS")]
	public class ImportGPSObjects : FsmStateAction
	{

		[RequiredField]
		[Title("Prefab Database")]
		[ObjectType(typeof(PrefabDatabase))]
		[Tooltip("ARLocation PrefabDatabase file.")]
		public FsmObject prefabDatabase;

		[RequiredField]
		[Title("XML Data File")]
		[ObjectType(typeof(TextAsset))]
		[Tooltip("Location XML file from web editor.")]
		public FsmObject XMLfile;

		public override void Reset()
		{
			prefabDatabase = null;
			XMLfile = null;
		}

		// Code that runs on entering the state.
		public override void OnEnter()
		{
			//Logger.Log("Getting GPS objects");
#if !UNITY_EDITOR
			GameObject.FindGameObjectWithTag("GPSManager").GetComponent<GPSManager>().InitFromXML(prefabDatabase.Value as PrefabDatabase, XMLfile.Value as TextAsset);
#endif
			Finish();
		}
	}

}
