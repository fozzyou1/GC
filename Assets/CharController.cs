using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {

	// Move Speed
	public float walkSpeed = 15f;
	public float runSpeed = 30f;

	Rigidbody _rigidbody;
	Animator anim;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
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
		Vector3 targetLook = transform.position + new Vector3(hor, 0, ver);
		transform.LookAt(targetLook);
	}
}
