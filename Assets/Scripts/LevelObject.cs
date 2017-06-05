using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelObject : MonoBehaviour {

	public enum LevelObjectType{Box1, Spinner, Plank, Spring, Coin};
	public LevelObjectType levelObjectType;
	public bool isIntro;
	public float forceTimeTillNext;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		int zIntPart = (int)transform.position.z;
		float zFracPart = transform.position.z - zIntPart;
		//Debug.Log(transform.position.z + " - " + zIntPart + " - " + zFracPart);
		float gridStep = 0;
		do
		{
			gridStep = gridStep + (12f/60f);
			gridStep = ((float)Mathf.Floor(10000 * gridStep)) / 10000;
		} while(gridStep < (zFracPart-0.0001f));

		transform.position = new Vector3( transform.position.x,transform.position.y, (float)zIntPart + gridStep);  
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
		case LevelObjectType.Plank:
			return "Plank";
			break;
		case LevelObjectType.Spring:
			return "Spring";
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
