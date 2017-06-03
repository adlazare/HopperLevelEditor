using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelObject : MonoBehaviour {

	public enum LevelObjectType{Box1, Spinner, Coin};
	public LevelObjectType levelObjectType;
	public bool isIntro;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
