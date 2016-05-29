using UnityEngine;
using System.Collections;

public enum ItemStatus{
	None, Active
}

public class GCGarbageItem : MonoBehaviour {

	private ItemStatus status = ItemStatus.None;



	void OnTriggerEnter(Collider Other){
		if(Other.tag.Contains("Char")){
			status = ItemStatus.Active;

			Other.GetComponent<CharController>().OnCloseProp(this);
		}
	}

	void OnTriggerExit(Collider Other){
		if(Other.tag.Contains("Char")){
			status = ItemStatus.None;

			Other.GetComponent<CharController>().OnLostProp(this);
		}
	}

}
