using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolDefinition : MonoBehaviour {

	public LevelObject.LevelObjectType levelObjectType;
	public int poolSize;
	public enum PoolType{platform, collectible, obstacle};
	public PoolType poolType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
