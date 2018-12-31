using UnityEngine;
using System.Collections;

public class MonsterCtrl : MonoBehaviour {
	private Transform monsterTr;
	private Transform playerTr;
	private NavMeshAgent nvAgent;

	// Use this for initialization
	void Start () {
		// 몬스터의 Transform 할당
		monsterTr = this.gameObject.GetComponent<Transform> ();
		// 추적 대상인 Player의 Transform 할당
		playerTr = GameObject.FindWithTag ("Player").GetComponent<Transform> ();
		// NavMeshAgent 컴포넌트 할당
		nvAgent = this.gameObject.GetComponent<NavMeshAgent> ();

		// 추적 대상의 위치를 설정하면 바로 추적 시작
		nvAgent.destination = playerTr.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
