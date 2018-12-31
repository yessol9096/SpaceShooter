using UnityEngine;
using System.Collections;

public class WallCtrl : MonoBehaviour {

	public GameObject sparkEffect;

	// 충돌을 시작할 때 발생하는 이벤트
	void OnCollisionEnter(Collision coll)
	{
		if (coll.collider.tag == "BULLET") {

			//GameObject spark = (GameObject) Instantiate(sparkEffect, coll.transform.position, Quaternion.identity);
			// 컴포넌트의 수행시간이 지난 후 삭제 처리
			//Destroy (spark, spark.GetComponent<ParticleSystem>().duration + 0.2f);

			Destroy (coll.gameObject);
		}
	}
}
