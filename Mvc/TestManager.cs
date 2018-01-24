using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour {

	// private const string MAP = "11111111," +
	// 						"10020001," +
	// 						"10333301," +
	// 						"10000001," +
	// 						"11111111";
	private const string MAP = "00000000," +
								"00020000," +
								"00000000," +
								"00000000," +
								"00000000";
							
	public MoverioUnityPlugin moverioUnityPlugin;

	private  GameObject mainCamera;

	public GameObject field;

	private GameObject player;
	public GameObject playerPrefab;

	public GameObject wall;
	public GameObject wallPrefab;

	public GameObject deskPrefab;


	private enum ObjectType
	{
		Empty=0,
		Wall=1,
		Player=2,
		Desk=3

	}

	private GameState gameState;
	private enum GameState
	{
		Title,
		Play,
		Pause,
		End
	}
	// Use this for initialization
	void Start () {
		moverioUnityPlugin.SensorData_HeadSet_Tap += SensorData_HeadSet_Tap;
		moverioUnityPlugin.SensorData_Controller_Shake += SensorData_Controller_Shake;

		CreateLabyrintWall ();
		//Caution ();

		//wallClear = WallClear.None;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnDisable(){

		moverioUnityPlugin.SensorData_HeadSet_Tap -= SensorData_HeadSet_Tap;
		moverioUnityPlugin.SensorData_Controller_Shake -= SensorData_Controller_Shake;

	}
	//==============================
	void CreateLabyrintWall(){

		string[] matrixs = MAP.Split(',');

		for(int z = 0; z < matrixs.Length; z++){
			string matrix_X = matrixs[z];
			for(int x = 0; x < matrix_X.Length; x++){
				int index = int.Parse(matrix_X.Substring(x, 1));
				if (index == (int)ObjectType.Wall) {
					GameObject prefab = Instantiate (wallPrefab, new Vector3 (x, 0, z), Quaternion.identity);
					prefab.transform.parent = wall.transform;
				} else if (index == (int)ObjectType.Player) {
					player = Instantiate (playerPrefab, new Vector3 (x, 0, z), Quaternion.identity);
					player.transform.parent = field.transform;
				} else if (index == (int)ObjectType.Desk) {
					GameObject prefab = Instantiate (deskPrefab, new Vector3 (x, 0, z), Quaternion.identity);
					prefab.transform.parent = wall.transform;
				}
					
			}
		}
	}
	//================================================
	private void SensorData_HeadSet_Tap(){

		if (gameState == GameState.Play) {
			// return;
			StartCoroutine (WaitWalk ());

			//shot.Play ();

			player.GetComponentInChildren<Shoot> ().Fire ();
		}

	}
	IEnumerator WaitWalk(){

		Rigidbody rigidbody = player.GetComponent<Rigidbody> ();

		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		player.GetComponent<HeadSetWalkFilter> ().enabled = false;

		yield return new WaitForSeconds (1.0f);

		player.GetComponent<HeadSetWalkFilter> ().enabled = true;

	}
	private void SensorData_Controller_Shake(float speed){

		if(gameState == GameState.Play){
			Animator animator = player.GetComponentInChildren<Animator> ();
			animator.SetTrigger ("Swing");

			//swing.Play ();

			StartCoroutine (WaitWalk());

		}

	}
}
