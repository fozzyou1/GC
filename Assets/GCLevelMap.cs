using UnityEngine;
using System.Collections;

public class GCLevelMap : MonoBehaviour {

	public GCItemCreator gcCreator;

	// Use this for initialization
	void Start () {
		gcCreator.SetGarbage();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
