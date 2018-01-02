﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {


	Rigidbody rigidBody;

	AudioSource audio; 

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInput ();
	}

	private void ProcessInput() {
		if (Input.GetKey (KeyCode.Space)) {
			rigidBody.AddRelativeForce (Vector3.up);

			if (audio.isPlaying == false) {
				audio.Play ();
			}
		} else {
			audio.Stop ();
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (Vector3.forward);
		}

		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (-Vector3.forward);
		}
	}
}
