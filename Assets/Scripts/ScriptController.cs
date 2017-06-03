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
			// TODO remove
			streamWriter.WriteLine("platformPoolCount,2");
			streamWriter.WriteLine("definePlatformPool,Box1,10");
			streamWriter.WriteLine("definePlatformPool,Spinner,10");


			List<GameObject> unfilteredObjectList = new List<GameObject>();
			List<GameObject> filteredObjectList = new List<GameObject>();
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
			}

			for(int i = 0; i < filteredObjectList.Count; i++)
			{
				GameObject go = filteredObjectList[i];
				LevelObject levelObject = go.GetComponent<LevelObject>();

				switch(levelObject.levelObjectType)
				{
				case LevelObject.LevelObjectType.Box1:
					writeBox1ScriptLine(go, levelObject, streamWriter);
					break;
				case LevelObject.LevelObjectType.Spinner:
					writeSpinnerScriptLine(go, levelObject, streamWriter);
					break;
				case LevelObject.LevelObjectType.Coin:
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

	private void writeBox1ScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform,0," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + ",1");
		}
	}

	private void writeSpinnerScriptLine(GameObject go, LevelObject levelObject, StreamWriter streamWriter)
	{
		string xPos = go.transform.position.x.ToString();
		string yPos = go.transform.position.y.ToString();
		string zPos = go.transform.position.z.ToString();
		string xRot = go.transform.rotation.eulerAngles.x.ToString();
		string yRot = go.transform.rotation.eulerAngles.y.ToString();
		string zRot = go.transform.rotation.eulerAngles.z.ToString();

		if(levelObject.isIntro)
		{
			streamWriter.WriteLine("introPlatform,1," + xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + ",1");
		}
	}
}
