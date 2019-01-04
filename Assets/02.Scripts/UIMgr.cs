using UnityEngine;
using System.Collections;

public class UIMgr : MonoBehaviour {

	public void OnClickStartBtn(RectTransform rt)
	{
		Debug.Log ("Scale X :" + rt.localScale.x.ToString());
	}
}
