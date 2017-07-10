using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelObject : MonoBehaviour {

	public enum LevelObjectType{Box1, Set1Spinner, Plank, Spring, Coin, Wall, PlankJr, Set1WoodBlock, Set1Spring, Set1Plank, Set1Wall, Set1EndBox, 
							    CoinPlus, Set1Spike, CameraAngle, ResetCamera, Set1MovingBlock, Set1MovingBlockReverse, Set1HalfPlank, Set1QuarterPlank,
								Set2Plank1,Set2Plank2,Set2WoodBlock };
	public LevelObjectType levelObjectType;
	public bool isIntro;
	public float forceTimeTillNext;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		snapToGrid();
	}

	private void snapToGrid()
	{
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
		case LevelObjectType.Set1Spinner:
			return "Set1Spinner";
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
		case LevelObjectType.Wall:
			return "Wall";
			break;
		case LevelObjectType.PlankJr:
			return "PlankJr";
			break;
		case LevelObjectType.Set1WoodBlock:
			return "Set1WoodBlock";
			break;
		case LevelObjectType.Set1Spring:
			return "Set1Spring";
			break;
		case LevelObjectType.Set1Plank:
			return "Set1Plank";
			break;
		case LevelObjectType.Set1Wall:
			return "Set1Wall";
			break;
		case LevelObjectType.Set1EndBox:
			return "Set1EndBox";
			break;
		case LevelObjectType.CoinPlus:
			return "CoinPlus";
			break;
		case LevelObjectType.Set1Spike:
			return "Set1Spike";
			break;
		case LevelObjectType.CameraAngle:
			return "CameraAngle";
			break;
		case LevelObjectType.ResetCamera:
			return "ResetCamera";
			break;
		case LevelObjectType.Set1MovingBlock:
			return "Set1MovingBlock";
			break;
		case LevelObjectType.Set1MovingBlockReverse:
			return "Set1MovingBlockReverse";
			break;
		case LevelObjectType.Set1HalfPlank:
			return "Set1HalfPlank";
			break;
		case LevelObjectType.Set1QuarterPlank:
			return "Set1QuarterPlank";
			break;

		case LevelObjectType.Set2Plank1:
			return "Set2Plank1";
			break;
		case LevelObjectType.Set2Plank2:
			return "Set2Plank2";
			break;
		case LevelObjectType.Set2WoodBlock:
			return "Set2WoodBlock";
			break;
		default:
			return "";
			break;
		}
	}
}
