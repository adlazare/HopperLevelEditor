using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelObject : MonoBehaviour {

	public enum LevelObjectType{Box1, Spinner, Coin};
	public LevelObjectType levelObjectType;
	public bool isIntro;
	public float forceTimeTillNext;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static string getObjectNameByObjectType(LevelObjectType lot)
	{
		// This name must match the prefab name in Unity scene
		switch(lot)
		{
		case LevelObjectType.Box1:
			return "Box1";
			break;
		case LevelObjectType.Spinner:
			return "Spinner";
			break;
		case LevelObjectType.Coin:
			return "Coin";
			break;
		default:
			return "";
			break;
		}
	}
}
