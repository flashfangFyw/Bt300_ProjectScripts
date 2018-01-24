using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using UnityEngine.UI;


/* 
    Author:     fyw 
    CreateDate: 2018-01-23 14:29:05 
    Desc:       注释 
*/


public class ControllerTracked : MonoBehaviour 
{

	#region public property

	public MoverioUnityPlugin moverioUnityPlugin;
    public Text Text1;
    public Text Text2;
    public Text Text3;
    #endregion
	#region private property
    #endregion

    #region unity function
    void OnEnable()
    {
    }
    void Start () 
	{
        moverioUnityPlugin.SensorData_Controller_Orientation += SensorData_Controller_Orientation;
        moverioUnityPlugin.SensorData_Controller_Gyroscope+=SensorData_Controller_Gryo;
    }   
	void Update () 
	{
	}
    void OnDisable()
    {
        moverioUnityPlugin.SensorData_Controller_Orientation -= SensorData_Controller_Orientation;
    }
    void OnDestroy()
    {
    }
    #endregion

	#region public function
	#endregion
	#region private function
    private void SetTxtContent(Vector3 quaternion)
    {
        if(Text1)  Text1.text=quaternion.x+"";
        if(Text2)  Text2.text=quaternion.y+"";
        if(Text3)  Text3.text=quaternion.z+"";
    }
	#endregion

    #region event function

	private void SensorData_Controller_Orientation(Quaternion quaternion){

		//Rotation Direct Value
		//this.gameObject.transform.localRotation = Quaternion.Slerp(this.gameObject.transform.localRotation,quaternion,Time.deltaTime);
        this.gameObject.transform.rotation = quaternion* Quaternion.AngleAxis (180.0f, Vector3.up);//Quaternion.AngleAxis (90.0f, Vector3.right) * quaternion ;//* Quaternion.AngleAxis (180.0f, Vector3.forward);
    
    return;
		// Vector3 localEulerAngles = new Vector3 (this.gameObject.transform.localEulerAngles.x, this.gameObject.transform.localEulerAngles.y, this.gameObject.transform.localEulerAngles.z);

		// this.gameObject.transform.localRotation = quaternion;

		// Vector3 vector3 = new Vector3 (localEulerAngles.x, 0, localEulerAngles.z);

		// if ((this.gameObject.transform.localEulerAngles.x > 0 && this.gameObject.transform.localEulerAngles.x <= 70) ||
		// 	(this.gameObject.transform.localEulerAngles.x > 290 && this.gameObject.transform.localEulerAngles.x <= 360)) {
		// 	vector3 = new Vector3 (-this.gameObject.transform.localEulerAngles.x,vector3.y,vector3.z);
		// }

		// if ((this.gameObject.transform.localEulerAngles.z > 0 && this.gameObject.transform.localEulerAngles.z <= 45) ||
		// 	(this.gameObject.transform.localEulerAngles.z > 315 && this.gameObject.transform.localEulerAngles.z <= 360)) {
		// 	vector3 = new Vector3 (vector3.x,vector3.y,-this.gameObject.transform.localEulerAngles.z);
		// }

		// this.gameObject.transform.localEulerAngles = vector3;
        SetTxtContent(this.gameObject.transform.localEulerAngles);
	}
    private void SensorData_Controller_Gryo(float[] vector)
    {
        return;
         if(Text1)  Text1.text=vector[0]+"";
        if(Text2)  Text2.text=vector[1]+"";
        if(Text3)  Text3.text=vector[2]+"";
        this.gameObject.transform.rotation = Quaternion.AngleAxis (90.0f, Vector3.right) * Input.gyro.attitude * Quaternion.AngleAxis (180.0f, Vector3.forward);
		//_t.localRotation = Quaternion.Slerp(transform.localRotation, Input.gyro.attitude *  Quaternion.Euler(new Vector3(0,0,180)), Time.deltaTime*30);
    }
    #endregion
}
