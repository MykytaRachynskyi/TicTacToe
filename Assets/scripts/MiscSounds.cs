﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MiscSounds : MonoBehaviour {
	public AudioClip click;
	AudioSource audio;

	// Use this for initialization
	void Start () {
		 audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Click()
	{
		audio.clip = click;
        audio.Play();
	}
}
