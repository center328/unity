using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public GameObject ground;
	public GameObject fence;
	public GameObject viewportCamera;
	
	public GUIText fenceHeightDisplay;
	public GUIText distanceXDisplay;
	public float distanceValue;

	public GameObject player1Capsule;
	public GameObject player2Capsule;

	public PlayerController activePlayer;

	public bool isPlayer1 = false;

	public const float MIN_VELOCITY = 10.0f;
	public const float MIN_ANGLE = 5.0f;
	public const float MAX_VELOCITY = 40.0f;
	public const float MAX_ANGLE = 85.0f;

	public const float ANGLE_INC = 0.4f;
	public const float VELOCITY_INC = 0.1f;

	public const float ZOOM_INC_Z = 0.4f;
	public const float ZOOM_INC_Y = 0.2f;
	public const float SCROLL_INC = 0.4f;

	public const float FENCE_HEIGHT = 11f;

	// Use this for initialization
	void Start () {
//		Debug.Log("start");

		distanceValue = Vector3.Distance (player1Capsule.transform.position, player2Capsule.transform.position);
		distanceXDisplay.text = "Distance X: " + distanceValue.ToString () + " m";
		fenceHeightDisplay.text = "Fence Height: " + fence.transform.lossyScale.y + " m";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftArrow)) {
	
			if (activePlayer.isControlActive) {
				if (!isPlayer1 && activePlayer.fireVelocity < MAX_VELOCITY) {
					activePlayer.fireVelocity += VELOCITY_INC;
				}
			} else {
				viewportCamera.transform.Translate (new Vector2 (-1f * SCROLL_INC, 0));
			}
		} else if (Input.GetKey (KeyCode.RightArrow)) {

			if (activePlayer.isControlActive) { 
				if (!isPlayer1 && activePlayer.fireVelocity > MIN_VELOCITY) {
					activePlayer.fireVelocity -= VELOCITY_INC;
				}
			} else {
				viewportCamera.transform.Translate (new Vector2 (SCROLL_INC, 0));
			}
		} else if ( Input.GetKey (KeyCode.UpArrow) ) {

			if (activePlayer.isControlActive) {
				if (!isPlayer1 && activePlayer.fireAngle > -1f * MAX_ANGLE) {
					activePlayer.fireAngle -= ANGLE_INC;
				}
			} else {
				viewportCamera.transform.Translate (new Vector3 (0, -1f * ZOOM_INC_Y, ZOOM_INC_Z));
			}
		} else if ( Input.GetKey (KeyCode.DownArrow) ) {

			if (activePlayer.isControlActive) {
				if (!isPlayer1 && activePlayer.fireAngle < -1f * MIN_ANGLE) {
					activePlayer.fireAngle += ANGLE_INC;
				}
			} else {
				viewportCamera.transform.Translate (new Vector3 (0, ZOOM_INC_Y, -1f * ZOOM_INC_Z));
			}
		}

		// Toggle calculator state
		if (Input.GetKeyDown (KeyCode.Return)) {

			int step = activePlayer.CURRENT_STEP++;
			if (step == 4) {
				activePlayer.CURRENT_STEP = 0;
			}
			
			Debug.Log("Enter key pressed " + activePlayer.CURRENT_STEP);
		}

		if (Input.GetMouseButtonUp (0) && !activePlayer.isMousedOver) {

			Debug.Log("Global click");
			player1Capsule.renderer.material.SetColor ("_Color",Color.gray);
			activePlayer.isControlActive = false;
			// turn of particles
			activePlayer.sightLineParticleSystem.GetComponent<ParticleSystem> ().renderer.enabled = false;
		}

		if (player2Capsule.activeInHierarchy == false) {
			player2Capsule.SetActive (true);
			Vector3 enemyPosition = new Vector3(945 + (40 * Random.value), 1, 1015);
			player2Capsule.transform.position = enemyPosition;
			//player2Capsule.nextup()
		}
	}
}
