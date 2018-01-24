using UnityEngine;  
using System.Collections;  
using System;
using UnityEngine.UI;

public class ShowGSensorValue : MonoBehaviour  
{  
  
   public  Text text1;
   public  Text text2;
   public  Text text3;

   public Canvas news;
    // Use this for initialization  
    void Start()  
    {  
  		InitStart();
    }  
  
    // Update is called once per frame  
    void Update()  
    {  
		// UpdateParams();
		UpdateNews();
    }  
  
    void OnGUI()  
    {  
        // GUI.Box(new Rect(5, 5, 1000, 100), String.Format("{0:0.000}", Input.acceleration.x));  
        // GUI.Box(new Rect(5, 110, 1000, 100), String.Format("{0:0.000}", Input.acceleration.y));  
        // GUI.Box(new Rect(5, 210, 1000, 100), String.Format("{0:0.000}", Input.acceleration.z));  
    }  
	private void InitStart()
	{
		Input.compass.enabled = true;
			this.transform.rotation=Quaternion.Euler(0,Input.compass.trueHeading+90,0);
		// enable the gyro
		Input.gyro.enabled = true;
	
	
		//  StartCoroutine(StartGPS());  
	}
	private void UpdateParams()
	{ 
		return;
		if( text1) text1.text=getInput().x+"";
		if( text2) text2.text=getInput().y+"";
		if( text3) text3.text=getInput().z+"";

	}
	private void UpdateNews()
	{
		// Orient an object to point northward.
        // transform.rotation = Quaternion.Euler(0, -Input.compass.trueHeading, 0);
		// read the magnetometer / compass value and generate a quaternion
		Quaternion trueHeading =Quaternion.Euler(0, -Input.compass.trueHeading, 0);
		// interpolate between old and new position
		transform.localRotation = Quaternion.Slerp (this.transform.localRotation, trueHeading, 0.05f);	
		// if(news) news.transform.rotation=Quaternion.Euler(0,Input.compass.trueHeading,0);
		// transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,Input.compass.magneticHeading, 0),Time.deltaTime*2);
	}
		private Vector3 getInput()
		{
			Vector3 v=Input.acceleration;
			v=Input.gyro.gravity;
			
			return v;
		}
		IEnumerator StartGPS () {  
				if(text1) text1.text = "开始获取GPS信息";  
		
				// 检查位置服务是否可用  
				if (!Input.location.isEnabledByUser) {  
					if(text2) text2.text = "位置服务不可用";  
					yield break;  
				}  
		
				// 查询位置之前先开启位置服务  
				if(text3) text3.text= "启动位置服务";  
				Input.location.Start ();   
		
				// 等待服务初始化  
				int maxWait = 60;  
				while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {  
					if(text1) text1.text = Input.location.status.ToString () + ">>>" + maxWait.ToString ();  
					yield return new WaitForSeconds (1);  
					maxWait--;  
				}  
		
				// 服务初始化超时  
				// if (maxWait < 1) {  
				// 	if(text1) text1.text = "服务初始化超时";  
				// 	yield break;  
				// }  
		
				// 连接失败  
				if (Input.location.status == LocationServiceStatus.Failed) {  
					if(text1) text1.text = "无法确定设备位置";  
					yield break;  
				} else {  
					if(text1) text1.text = "Location: rn" +  
					"纬度：" + Input.location.lastData.latitude + "rn" +  
					"经度：" + Input.location.lastData.longitude + "rn" +  
					"海拔：" + Input.location.lastData.altitude + "rn" +  
					"水平精度：" + Input.location.lastData.horizontalAccuracy + "rn" +  
					"垂直精度：" + Input.location.lastData.verticalAccuracy + "rn" +  
					"时间戳：" + Input.location.lastData.timestamp;  
				}  
		
				// 停止服务，如果没必要继续更新位置，（为了省电）  
			// 	Input.location.Stop ();  
			// //========================
			// 	// Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置  
			// 	// LocationService.isEnabledByUser 用户设置里的定位服务是否启用  
			// 	if (!Input.location.isEnabledByUser) {  
			// 		if(text1) text1.text = "isEnabledByUser value is:"+Input.location.isEnabledByUser.ToString()+" Please turn on the GPS";   
			// 		return false;  
			// 	}  
		
			// 	// LocationService.Start() 启动位置服务的更新,最后一个位置坐标会被使用  
				// Input.location.Start(10.0f, 10.0f);  
		
			// 	int maxWait = 20;  
			// 	while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {  
			// 		// 暂停协同程序的执行(1秒)  
			// 		yield return new WaitForSeconds(1);  
			// 		maxWait--;  
			// 	}  
		
			// 	if (maxWait < 1) {  
			// 	if(text1) text1.text = "Init GPS service time out";  
			// 		return false;  
			// 	}  
		
			// 	if (Input.location.status == LocationServiceStatus.Failed) {  
			// 		if(text1) text1.text= "Unable to determine device location";  
			// 		return false;  
			// 	}   
			// 	else {  
			// 		if(text1) text1.text = "N:" + Input.location.lastData.latitude ;  
			// 		if(text2) text2.text = " E:"+Input.location.lastData.longitude;
			// 		if(text3) text3.text =  " Time:" + Input.location.lastData.timestamp;  
			// 		yield return new WaitForSeconds(100);  
			// 	}  
		}  
} 