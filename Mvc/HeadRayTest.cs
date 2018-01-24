using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ffDevelopmentSpace;


public class HeadRayTest : MonoBehaviour {

 	private Camera _camera;
    private Camera thisCamera
    {
        get{
            if (_camera == null) _camera = this.gameObject.GetComponent<Camera>();
            return _camera;
        }
    }
	 public float maxRayDistance = 30.0f;
    Ray ray;  
    RaycastHit hit;  
    Vector3 center = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);  
    Vector3 hitpoint = Vector3.zero;  
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		 ray = thisCamera.ScreenPointToRay(center);  
        if (Physics.Raycast(ray, out hit, maxRayDistance))  
        {  
           Singleton<EyesOnModel>.GetInstance().EyeOnNewObject(hit);
		}
		
	}
}
