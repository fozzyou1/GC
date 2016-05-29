using UnityEngine;
using System.Collections;

public class GCGenPoint : MonoBehaviour {

	public GameObject createItemPrefab;
	public float distance = 3;

	private GCGarbageItem createItem = null;

	// Use this for initialization
	void Start () {
		
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, distance);
	}
	
	public void CreateItem(){
		float x = Random.Range(-1f, 1f);
		float z = Random.Range(-1f, 1f);
		Vector3 direction = new Vector3(x, 0, z);

		GameObject obj = Instantiate(createItemPrefab) as GameObject;
		obj.transform.position = transform.position + Random.Range(0, distance) * direction;
		obj.transform.parent = transform;

		createItem = obj.GetComponent<GCGarbageItem>();
	}
}
