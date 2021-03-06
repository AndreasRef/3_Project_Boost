﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 100f;
	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip success;
	[SerializeField] AudioClip death;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem succesParticles;
	[SerializeField] ParticleSystem deathParticles;

	Rigidbody rigidBody;

	AudioSource audioSource; 

	enum State { Alive, Dying, Transcending }
	State state  = State.Alive;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.Alive) {
			RespondToThrustInput ();
			RespondToRotateInput ();
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
			StartSuccessSequence ();
			break;
		default:
			StartDeathSequence();
			break;
		}
	}

	private void StartSuccessSequence() {
		state =  State.Transcending;
		audioSource.Stop ();
		audioSource.PlayOneShot (success);
		succesParticles.Play();
		Invoke ("LoadNextLevel", 2f);;
	}

	private void StartDeathSequence() {
		state = State.Dying;
		audioSource.Stop ();
		audioSource.PlayOneShot (death);
		deathParticles.Play ();
		Invoke ("LoadFirstLevel", 1f);;
	}


	private void LoadNextLevel () {
		deathParticles.Stop ();
		SceneManager.LoadScene (1);
	}

	private void LoadFirstLevel () {
		SceneManager.LoadScene (0);
	}


	private void RespondToThrustInput ()
	{
		float thrustThisFrame = mainThrust * Time.deltaTime;
		if (Input.GetKey (KeyCode.Space)) {
						ApplyThrust ();
		}
		else {
			audioSource.Stop ();
			mainEngineParticles.Stop ();
		}
	}

	void ApplyThrust ()
	{
		rigidBody.AddRelativeForce (Vector3.up * mainThrust * Time.deltaTime);
		if (audioSource.isPlaying == false) {
			audioSource.PlayOneShot (mainEngine);
		}
		mainEngineParticles.Play ();
	}

	private void RespondToRotateInput ()
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
