using UnityEngine;
using System.Collections;

public class DelegateTest : MonoBehaviour {

	delegate void CalNumDelegate(int num);

	CalNumDelegate calNum;

	// Use this for initialization
	void Start () {
		calNum = OnePlusNum;
		calNum (4);

		calNum = PowerNum;
		calNum (5);
	}

	void OnePlusNum(int num)
	{	
		int result = num + 1;
		Debug.Log ("One Plus = " + result);
	}

	void PowerNum(int num)
	{
		int result = num * num;
		Debug.Log ("Power =" + result);
	}

}
