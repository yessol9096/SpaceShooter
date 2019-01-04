using UnityEngine;
using System.Collections;

public class MonsterCtrl : MonoBehaviour {
	// 몬스터의 상태 정보가 있는 Enumerable 변수 선언
	public enum MonsterState {idle, trace, attack, die};

	// 몬스터의 현재 상태 정보를 저장할 Enum 변수
	public MonsterState monsterState = MonsterState.idle;

	private Transform monsterTr;
	private Transform playerTr;
	private NavMeshAgent nvAgent;
	private Animator animator;

	//추적 사정거리
	public float traceDist = 10.0f;
	//공격 사정거리
	public float attackDist = 2.0f;

	//몬스터의 사망 여부
	private bool isDie = false;

	public GameObject bloodEffect;
	public GameObject bloodDecal;

	private int hp = 100;

	private GameUI gameUI;

	// Use this for initialization
	void Start () {
		// 몬스터의 Transform 할당
		monsterTr = this.gameObject.GetComponent<Transform> ();
		// 추적 대상인 Player의 Transform 할당
		playerTr = GameObject.FindWithTag ("Player").GetComponent<Transform> ();
		// NavMeshAgent 컴포넌트 할당
		nvAgent = this.gameObject.GetComponent<NavMeshAgent> ();

		animator = this.gameObject.GetComponent<Animator> ();
		// 추적 대상의 위치를 설정하면 바로 추적 시작
		//nvAgent.destination = playerTr.position;

		//일정한 간격으로 몬스터의 행동 상태를 체크하는 코루틴 함수 실행
		StartCoroutine (this.CheckMonsterState ());

		//몬스터의 상태에 따라 동작하는 루틴을 실행하는 코루틴 함수 실행
		StartCoroutine (this.MonsterAction ());

		gameUI = GameObject.Find ("GameUI").GetComponent<GameUI> ();
	}

	IEnumerator CheckMonsterState()
	{
		while (!isDie) 
		{
			//0.2초 동안 기다렸다가 다음으로 넘어감
			yield return new WaitForSeconds(0.2f);

			//몬스터와 플레이어 사이의 거리 측정
			float dist = Vector3.Distance(playerTr.position, monsterTr.position);

			if (dist <= attackDist) // 공격거리 범위 이내로 들어왔는지 확인
			{
				monsterState = MonsterState.attack;
			}

			else if (dist <= traceDist) // 추적거리 범위 이내로 들어왔는지 확인
			{
				monsterState = MonsterState.trace;
			}

			else
			{
				monsterState = MonsterState.idle;	//몬스터의 상태를 idle 모드로 설정 
			}
		}
	}

	// 몬스터의 상태값에 따라 적절한 동작을 수행하는 함수
	IEnumerator MonsterAction()
	{
		while (!isDie) 
		{
			switch(monsterState)
			{
				//idle 상태 추적 중지
				case MonsterState.idle:
					nvAgent.Stop();
					animator.SetBool("IsTrace", false);
					break;

				//추적상태
				case MonsterState.trace:
					nvAgent.destination = playerTr.position;
					nvAgent.Resume();
					animator.SetBool("IsAttack", false);
					animator.SetBool("IsTrace", true);
					break;
			
				case MonsterState.attack:
					nvAgent.Stop ();
					animator.SetBool("IsAttack", true);
					break;
			}
			yield return null;
		}
	}

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.tag == "BULLET") 
		{
			CreateBloodEffect(coll.transform.position);

			hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;
			if(hp <= 0)
			{
				MonsterDie();
			}
			Destroy(coll.gameObject);
			animator.SetTrigger("IsHit");
		}
	}

	void MonsterDie()
	{
		StopAllCoroutines ();

		isDie = true;
		monsterState = MonsterState.die;
		nvAgent.Stop ();
		animator.SetTrigger ("IsDie");

		gameObject.GetComponentInChildren<CapsuleCollider> ().enabled = false;

		foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>()) 
		{
			coll.enabled = false;
		}

		gameUI.DispScore(50);
	}

	void CreateBloodEffect(Vector3 pos)
	{
		GameObject blood1 = (GameObject)Instantiate (bloodEffect, pos, Quaternion.identity);
		Destroy (blood1, 2.0f);

		Vector3 decalPos = monsterTr.position + (Vector3.up * 0.05f);
		//데칼의 회전값을 무작위로 설정
		Quaternion decalRot = Quaternion.Euler (90, 0, Random.Range (0, 360));

		GameObject blood2 = (GameObject)Instantiate (bloodDecal, decalPos, decalRot);
		float scale = Random.Range (1.5f, 3.5f);
		blood2.transform.localScale = Vector3.one * scale;

		Destroy (blood2, 5.0f);
	}

	void OnPlayerDie()
	{
		StopAllCoroutines ();
		nvAgent.Stop ();
		animator.SetTrigger ("IsPlayerDie");
	}

	void OnEnable()
	{
		PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
	}

	void OnDisable()
	{
		PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
	}

}
