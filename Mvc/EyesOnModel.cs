using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ffDevelopmentSpace;

public class EyesOnModel : ModelBase {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void EyeOnNewObject(RaycastHit hit)
	{
		 dispatchEvent(EyesOnEvent.EYES_ON_NEW_OBJ,hit.collider.gameObject.name);
	}
}
