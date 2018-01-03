using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 100f;

	Rigidbody rigidBody;

	AudioSource audio; 

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		Thrust ();
		Rotate ();
	}

	void OnCollisionEnter(Collision collision) {
		switch (collision.gameObject.tag) {
		case "Friendly": 
			//Do nothing
			print("OK");
			break;
		case "Fuel":
			print ("Fuel");
			break;
		default:
			print ("Dead");
			break;
		}
	}

	private void Thrust ()
	{
		float thrustThisFrame = mainThrust * Time.deltaTime;
		if (Input.GetKey (KeyCode.Space)) {
			rigidBody.AddRelativeForce (Vector3.up * thrustThisFrame);
			if (audio.isPlaying == false) {
				audio.Play ();
			}
		}
		else {
			audio.Stop ();
		}
	}

	private void Rotate ()
	{
		rigidBody.freezeRotation = true; //Take manual control of rotation


		float rotationThisFrame = rcsThrust * Time.deltaTime;

		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (Vector3.forward * rotationThisFrame);
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (-Vector3.forward * rotationThisFrame);
		}

		rigidBody.freezeRotation = true; //Remove manual control
	}
}
