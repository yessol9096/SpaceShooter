﻿using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	public Transform targetTr;	// 추적할 타깃 게임 오브젝트의 Transform 변수
	public float dist = 10.0f;  // 카메라와의 일정거리
	public float height = 3.0f; // 카메라의 높이 설정
	public float dampTrace = 20.0f; // 부드러우 추적을 위한 변수 

	//카메라 자신의 Transform 변수
	private Transform tr;


	// Use this for initialization
	void Start () {
		//카메라 자신의 Transform 컴포넌트를 tr에 할당
		tr = GetComponent<Transform> ();
	}

	//update 함수 호출 이후 한 번씩 호출되는 함수인 LateUpdate 사용
	// 추적할 타깃의 이동이 종료된 이후에 카메라가 추적하기 위해 LateUpdate 사용
	void LateUpdate() {
		//카메라의 위치를 추적대상의 dist 변수만큼 뒤쪽으로 배치하고 
		//height 변수만큼 위로 올림
		// lerp 는 선형보간 함수이다. 부드러운 이동을 위해서 추가 
		tr.position = Vector3.Lerp (tr.position		
		                            , targetTr.position - (targetTr.forward * dist) + (Vector3.up * height)
		                            , Time.deltaTime * dampTrace);
		//카메라가 타깃 게임 오브젝트를 바라보게 설정
		tr.LookAt (targetTr.position);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
