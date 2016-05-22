using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharController : MonoBehaviour {

	// Move Speed
	public float walkSpeed = 15f;
	public float runSpeed = 30f;

	public bool isDebug = false;

	Rigidbody _rigidbody;
	Animator anim;

	public List<CGGarbageItem> closePropList = new List<CGGarbageItem>();
	private CGGarbageItem curSelectProp = null;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
	}

	void Update(){
		SelectInPropsList();
	}
	
	void FixedUpdate(){

		float hor = Input.GetAxis("Horizontal");
		float ver = Input.GetAxis("Vertical");

		bool isRun = false;
		if(Input.GetKey(KeyCode.LeftCommand)){
			isRun = true;
		}

		anim.SetFloat("Speed", Mathf.Max(Mathf.Abs(hor), Mathf.Abs(ver)));
		anim.SetBool("Run", isRun);

		if(isRun){
			_rigidbody.velocity = new Vector3( hor, 0, ver ) * Time.deltaTime * runSpeed;

		}else{
			_rigidbody.velocity = new Vector3( hor, 0, ver ) * Time.deltaTime * walkSpeed;
		}

		CharRotation(hor, ver);
	}
		
	void CharRotation(float hor, float ver){
		if( !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || 
			Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) )
			return;

		Vector3 targetLook = transform.position + new Vector3(hor, 0, ver);
		transform.LookAt(targetLook);
	}

	public void OnCloseProp(CGGarbageItem item){
		closePropList.Add(item);

		SelectInPropsList();
	}

	public void OnLostProp(CGGarbageItem item){
		closePropList.Remove(item);

		SelectInPropsList();
	}

	// 가장 가까운 아이템을 
	void SelectInPropsList(){
		if(closePropList.Count <= 0){
			ChangeSelectProp ( null );
		}else if(closePropList.Count == 1){
			// 1개면 그냥
			ChangeSelectProp( closePropList[0] );
		}else{
			// 2개 이상이니 조건을 찾아야 한다.
			int minIndex = 0;
			float _min = 9999f;
			for (int i = 0; i < closePropList.Count; i++) {
				Vector3 charToItem = transform.position - closePropList[i].transform.position;
				float propAngle = Vector3.Dot(charToItem.normalized, transform.forward);
				print(i + ": " + propAngle);
				if( _min > propAngle )
				{
					_min = propAngle;
					minIndex = i;
				}
			}

			ChangeSelectProp(closePropList[minIndex]);
		}

		if(isDebug && curSelectProp != null)
			print("curSelectProp: " + curSelectProp);
	}

	void ChangeSelectProp(CGGarbageItem targetProp){
		if(targetProp == null)
			curSelectProp = null;
		
		if(curSelectProp != targetProp)
			curSelectProp = targetProp;
	}

}
