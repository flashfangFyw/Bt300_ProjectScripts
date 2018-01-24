using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ffDevelopmentSpace;
using UnityEngine.UI;

public class UIRiever : MonoBehaviour {

	public Text text;
	// Use this for initialization
	void Start () {
		Singleton<EyesOnModel>.GetInstance().addEvent(EyesOnEvent.EYES_ON_NEW_OBJ,OnEyesOn);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnEyesOn(EventObject e)
	{
	  	Debug.Log(e.obj.ToString());
		if(text) text.text="目标名称为"+e.obj.ToString();
	}
}
