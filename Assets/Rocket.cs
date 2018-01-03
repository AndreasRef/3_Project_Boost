using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 100f;

	Rigidbody rigidBody;

	AudioSource audio; 

	enum State { Alive, Dying, Transcending }
	State state  = State.Alive;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.Alive) {
			Thrust ();
			Rotate ();
		}
	}

	void OnCollisionEnter(Collision collision) {

		if (state != State.Alive) {
			return;
		}

		switch (collision.gameObject.tag) {
		case "Friendly": 
			//Do nothing
			print("OK");
			break;
		case "Finish":
			state =  State.Transcending;
			Invoke ("LoadNextLevel", 1f);;
			break;
		default:
			state = State.Dying;
			Invoke ("LoadFirstLevel", 1f);;
			break;
		}
	}

	private void LoadNextLevel () {
		SceneManager.LoadScene (1);
	}

	private void LoadFirstLevel () {
		SceneManager.LoadScene (0);
	}


	private void Thrust ()
	{
		float thrustThisFrame = mainThrust * Time.deltaTime;
		if (Input.GetKey (KeyCode.Space)) {
			//rigidBody.AddRelativeForce (Vector3.up * thrustThisFrame);

			rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

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
