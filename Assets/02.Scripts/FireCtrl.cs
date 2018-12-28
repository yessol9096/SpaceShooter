﻿using UnityEngine;
using System.Collections;

public class FireCtrl : MonoBehaviour {
	public GameObject bullet;
	public Transform firePos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Fire ();
		}
	}

	void Fire() {
		CreateBullet ();
	}

	void CreateBullet(){
		Instantiate (bullet, firePos.position, firePos.rotation);
	}
}
