using UnityEngine;
using System.Collections;

public class GCItemCreator : MonoBehaviour {

	[SerializeField]
	private GCGenPoint[] genPoints;

	// Use this for initialization
	void Awake () {
		genPoints = GetComponentsInChildren<GCGenPoint>();
	}

	public void SetGarbage(){
		for (int i = 0; i < genPoints.Length; i++) {
			genPoints[i].CreateItem();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
