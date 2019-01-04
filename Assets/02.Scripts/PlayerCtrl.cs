﻿using UnityEngine;
using System.Collections;

// inspector 뷰에 노출 하기 위함
[System.Serializable]
public class Anim
{
	public AnimationClip idle;
	public AnimationClip runForward;
	public AnimationClip runBackward;
	public AnimationClip runRight;
	public AnimationClip runLeft;
}


public class PlayerCtrl : MonoBehaviour {

	public delegate void PlayerDieHandler();
	public static event PlayerDieHandler OnPlayerDie;

	private float h = 0.0f;
	private float v = 0.0f;

	// 접근 해야 하는 컴포넌트는 반드시 변수에 할당한 후 사용
	private Transform tr;
	// 이동속도 변수
	public float moveSpeed = 10.0f;

	// 회전 속도 변수
	public float rotSpeed = 100.0f;


	// 인스펙터뷰에 표시할 애니메이션 클래스 변수
	public Anim anim;

	// 아래에 있는 3D 모델의 Animation 컴포넌트에 접근하기 위한 변수
	public Animation _animation;

	public int hp = 100;

	// Use this for initialization
	void Start () {
		//float vec1 = Vector3.Magnitude (Vector3.forward);
		//float vec2 = Vector3.Magnitude (Vector3.forward + Vector3.right);
		//float vec3 = Vector3.Magnitude ((Vector3.forward + Vector3.right).normalized); 
		//Debug.Log (vec1);
		//Debug.Log (vec2);
		//Debug.Log (vec3);
		// 스크립트 처음에 Transform 컴포넌트 할당
		tr = GetComponent<Transform> ();
	

		_animation = GetComponentInChildren<Animation> ();

		//Animation 컴포넌트의 애니메이션 클립을 지정하고 실행
		_animation.clip = anim.idle;
		_animation.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");

		//Debug.Log ("H=" + h.ToString ());
		//Debug.Log ("V=" + v.ToString ());

		//전후좌우 이동 방햑 벡터 계산
		Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

		//Translate(이동 방향 * 속도 * 변위값 * Time.deltaTime, 기준좌표 )
		tr.Translate (moveDir.normalized * Time.deltaTime * moveSpeed, Space.Self);

		//Vector3.up 축을 기준으로 rotSpeed만큼의 속도로 회전
		tr.Rotate (Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis ("Mouse X"));

		//키보드 입력값을 기준으로 동작할 애니메이션 수행
		if (v >= 0.1f) {
			//전진 애니메이션
			_animation.CrossFade (anim.runForward.name, 0.1f);
		} else if (v <= -0.1f) {
			//후진 애니메이션
			_animation.CrossFade (anim.runBackward.name, 0.1f);
		} else if (h >= 0.1f) {
			//오른쪽 이동 애니메이션
			_animation.CrossFade (anim.runRight.name, 0.1f);
		} else if (h <= -0.1f) {
			//왼쪽 이동 애니메이션
			_animation.CrossFade (anim.runLeft.name, 0.1f);
		} else {
			//정지시 idle 애니메이션
			_animation.CrossFade (anim.idle.name, 0.1f);
		}

	}

	void OnTriggerEnter(Collider coll)
	{
		//충돌한 Collider가 몬스터의 PUNCH이면 player의 HP 차감
		if(coll.gameObject.tag== "PUNCH")
		{
			hp -= 10;
			Debug.Log("Player HP =" + hp.ToString());

			//player의 생명이 0 이하이면 사망처리
			if (hp <= 0)
			{
				PlayerDie();
			}

		}
	}

	void PlayerDie()
	{
		Debug.Log ("Player Die!!");

		//MONSTER TAG를 가진 모든 게임오브젝트를 찾아옴
		//GameObject[] monsters = GameObject.FindGameObjectsWithTag ("MONSTER");

		//모든 몬스터의 OnPlayerDie 함수를 순차적으로 호출
		//foreach (GameObject monster in monsters) 
		//{
		//	monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
		//}
		OnPlayerDie ();
	}
}
