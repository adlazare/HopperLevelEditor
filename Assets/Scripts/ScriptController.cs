using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

[ExecuteInEditMode]
public class ScriptController : MonoBehaviour {

	private static ScriptController staticRefToThis;
	public static int buildCount = 0;
	public static bool errorHappened = false;
	public string path = "";

	[MenuItem("Custom Commands/Generate Script _b")]
	static void FirstCommand()
	{
		if(staticRefToThis == null)
		{
			Debug.Log("Couldn't build yet... try again now.");
		}
		else
		{
			buildCount++;
			Debug.Log("BUILDING SCRIPT");
			staticRefToThis.updateScript();
			if(errorHappened) { Debug.Log("BUILD FAILED!!!!!!!!!"); }
			else { Debug.Log("BUILD SUCCEEDED " + buildCount); }
		}
	}

	// Use this for initialization
	void Start () {
		UnityEditor.EditorApplication.isPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(staticRefToThis == null) staticRefToThis = this;
	}

	public void updateScript()
	{
		errorHappened = true;
		StreamWriter streamWriter = null;

		try
		{
			// Check if you can find the script file
			string filePath = path + "/script.txt";
			if(!File.Exists(filePath))
			{
				Debug.Log("File not found");
				return;
			}

			streamWriter = new StreamWriter(filePath, false);

			List<GameObject> unfilteredObjectList = new List<GameObject>();
			List<GameObject> filteredObjectList = new List<GameObject>();
			List<GameObject> poolDefinitionObjectList = new List<GameObject>();
			List<PoolDefinition> poolDefinitionList = new List<PoolDefinition>();
			UnityEngine.Object[] rawObjectList = FindObjectsOfType(typeof(GameObject));
			for(int i = 0; i < rawObjectList.Length; i++)
			{
				unfilteredObjectList.Add((GameObject)rawObjectList[i]);
			}

			// Black magic (sorts the list by z position)
			unfilteredObjectList.Sort((x,y) => x.transform.position.z.CompareTo(y.transform.position.z));
			for(int i = 0; i < unfilteredObjectList.Count; i++)
			{
				GameObject go = unfilteredObjectList[i];
				if(go.layer == 8)
				{
					filteredObjectList.Add(go);
				}
				else if(go.layer == 9)
				{
					poolDefinitionObjectList.Add(go);
				}
			}

			streamWriter.WriteLine("platformPoolCount," + poolDefinitionObjectList.Count.ToString());
			for(int i = 0; i < poolDefinitionObjectList.Count; i++)
			{
				PoolDefinition poolDefinition = poolDefinitionObjectList[i].GetComponent<PoolDefinition>();
				poolDefinitionList.Add(poolDefinition);

				switch(poolDefinition.poolType)
				{
				case PoolDefinition.PoolType.platform:
					streamWriter.WriteLine("definePlatformPool," +  LevelObject.getObjectNameByObjectType(poolDefinition.levelObjectType) + "," + poolDefinition.poolSize.ToString());
					break;
				case PoolDefinition.PoolType.collectible:
					break;
				case PoolDefinition.PoolType.obstacle:
					break;
				}
			}

			// Write the script lines for the level objects
			for(int i = 0; i < filteredObjectList.Count; i++)
			{
				GameObject go = filteredObjectList[i];
				float timeTillNext = 0;
				LevelObject levelObject = go.GetComponent<LevelObject>();
				if(i < (filteredObjectList.Count - 1))
				{
					GameObject nextObject = filteredObjectList[i + 1];
					timeTillNext = (nextObject.transform.position.z - levelObject.gameObject.transform.position.z) / 12f;
//					Debug.Log("Diff");
//					Debug.Log(nextObject.transform.position.z - levelObject.gameObject.transform.position.z);
//					Debug.Log("Time till next");
//					Debug.Log(timeTillNext);
//					Debug.Log("---------------------");
				}
				else
				{
					timeTillNext = 10;
				}

				switch(levelObject.levelObjectType)
				{
				case LevelObject.LevelObjectType.Box1:
					writeBox1ScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Set1Spinner:
					writeSet1SpinnerScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Plank:
					writePlankScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Spring:
					writeSpringScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Coin:
					writeCoinScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Wall:
					writeWallScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.PlankJr:
					writePlankJrScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Set1WoodBlock:
					writeSet1WoodBlockScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Set1Spring:
					writeSet1SpringScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Set1Plank:
					writeSet1PlankScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Set1Wall:
					writeSet1WallScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Set1EndBox:
					writeSet1EndBoxScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.CoinPlus:
					writeCoinPlusScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Set1Spike:
					writeSet1SpikeScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.CameraAngle:
					writeCameraAngleScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.ResetCamera:
					writeResetCameraScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Set1MovingBlock:
					writeSet1MovingBlockScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				case LevelObject.LevelObjectType.Set1MovingBlockReverse:
					writeSet1MovingBlockReverseScriptLine(go, levelObject, streamWriter, poolDefinitionList, timeTillNext.ToString());
					break;
				default:
					break;
				}
			}

			streamWriter.Close();
			errorHappened = false;
		}
		catch(Exception ex)
		{
			Debug.Log(ex.StackTrace);
		}
	}

	private void writeBox1ScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Box1).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}
	}

	private void writeSet1SpinnerScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Set1Spinner).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}

	}

	private void writePlankScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Plank).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}

	}

	private void writeSpringScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Spring).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}

	}

	private void writeCoinScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Coin).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}

	}

	private void writeWallScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Wall).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}
	}

	private void writePlankJrScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.PlankJr).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}
	}

	private void writeSet1WoodBlockScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Set1WoodBlock).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}
	}

	private void writeSet1SpringScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Set1Spring).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}
	}

	private void writeSet1PlankScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Set1Plank).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}
	}

	private void writeSet1WallScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Set1Wall).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}
	}

	private void writeSet1EndBoxScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Set1EndBox).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}
	}

	private void writeCoinPlusScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.CoinPlus).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}
	}

	private void writeSet1SpikeScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Set1Spike).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}
	}

	private void writeCameraAngleScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.CameraAngle).ToString();

		streamWriter.WriteLine("cameraAngle," + xPos + "," + yPos + "," + "-9.89" + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
	}

	private void writeResetCameraScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		streamWriter.WriteLine("resetCamera");
	}

	private void writeSet1MovingBlockScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Set1MovingBlock).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}
	}

	private void writeSet1MovingBlockReverseScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter, List<PoolDefinition> poolList, string timeTillNextString)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();
		string poolIndex = getPoolIndexByObjectType(poolList, LevelObject.LevelObjectType.Set1MovingBlockReverse).ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform," + poolIndex + "," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "," + levelObject.forceTimeTillNext.ToString());
		}
		else
		{
			streamWriter.WriteLine("platform," + poolIndex + "," + xPos + "," + yPos + "," + xRot + "," + yRot + "," + zRot + "," + timeTillNextString);
		}
	}

	private int getPoolIndexByObjectType(List<PoolDefinition> poolList, LevelObject.LevelObjectType lot)
	{
		for(int i = 0; i < poolList.Count; i++)
		{
			if(poolList[i].levelObjectType == lot)
			{
				return i;
			}
		}

//		Debug.Log("Pool not found for this object: " + lot.ToString());
		return 0;
	}
}
