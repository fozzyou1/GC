using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharController : MonoBehaviour {

	// Move Speed
	public float walkSpeed = 15f;
	public float runSpeed = 30f;

	public float jumpSpeed = 5f;
	public float gravity = 9.8f;
	public float ySpeed = 0;

	public bool isJump = false;

	public bool isDebug = false;

	CharacterController _charCtrl;
	Animator anim;

	public List<GCGarbageItem> closePropList = new List<GCGarbageItem>();
	private GCGarbageItem curSelectProp = null;

	// Use this for initialization
	void Start () {
		_charCtrl = GetComponent<CharacterController>();
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

		if(Input.GetKeyDown(KeyCode.Space)){
			Jump();
		}

		anim.SetFloat("ySpeed", (ySpeed * 0.1f));
		anim.SetFloat("Speed", Mathf.Max(Mathf.Abs(hor), Mathf.Abs(ver)));
		anim.SetBool("Run", isRun);

		float curSpeed = walkSpeed;
		if(isRun){
			curSpeed = runSpeed;
		}

		// Falling Speed
		if( IsGrounded() ){
			ySpeed = 0;
			isJump = false;
		}else{
			ySpeed -= gravity * Time.deltaTime;
			
		}

		Vector3 MoveVector = new Vector3( hor, ySpeed, ver ) * curSpeed;

		// Move
		_charCtrl.Move(MoveVector * Time.deltaTime);

		CharRotation(hor, ver);
	}

	public bool IsGrounded(){
		return (IsGroundedByCController() || IsGroundedByRaycast()) && ySpeed < 0;      //this also doesn't call raycast if we know we are grounded

	}

	public bool IsGroundedByCController()
	{
		if(_charCtrl.isGrounded) 	
			return true;
		else
			return false;
	}

	bool IsGroundedByRaycast(){
		RaycastHit hit;
		Debug.DrawRay(transform.position, (-Vector3.up * .1f), Color.green);       //draw the line to be seen in scene window

		if(Physics.Raycast(transform.position, -Vector3.up, out hit, 0.1f)){      //if we hit something
			return true;
		}
		return false;
	}

	void Jump(){
		ySpeed = jumpSpeed;
		isJump = true;
	}
		
	void CharRotation(float hor, float ver){
		if( !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || 
			Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) ){
			//print("Return");
			return;
		}

		Vector3 targetLook = transform.position + new Vector3(hor, 0, ver);
		//print("rotation " + targetLook);
		transform.LookAt(targetLook);
	}

	public void OnCloseProp(GCGarbageItem item){
		closePropList.Add(item);

		SelectInPropsList();
	}

	public void OnLostProp(GCGarbageItem item){
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

	void ChangeSelectProp(GCGarbageItem targetProp, bool direct = true){
		if(targetProp == null){
			curSelectProp = null;
			return;
		}

		if(direct){
			GetProp(targetProp);

			return;
		}

		if(curSelectProp != targetProp)
			curSelectProp = targetProp;
	}

	void GetProp(GCGarbageItem prop){
		if(isDebug)
			print("Get " + prop.name);
		
		closePropList.Remove(prop);
		Destroy(prop.gameObject);
	}

}
