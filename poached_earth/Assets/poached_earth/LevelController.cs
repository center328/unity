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

	public GUIText whoIsPlayingDisplay;
	const string whoIsText1 = "Player 1!";
	const string whoIsText2 = "Player 2!";

	public PlayerController player1;
	public PlayerController player2;

	public bool isPlayer1 = true;

	public const float MIN_VELOCITY = 10.0f;
	public const float MIN_ANGLE = 5.0f;
	public const float MAX_VELOCITY = 40.0f;
	public const float MAX_ANGLE = 85.0f;
	
	public const float DEFAULT_ANGLE_1 = -45.0f;

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

		isPlayer1 = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isPlayer1) {
			whoIsPlayingDisplay.text = LevelController.whoIsText1;
		} else {
			whoIsPlayingDisplay.text = LevelController.whoIsText2;
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
	

			if (!isPlayer1) {
				if (player2.isControlActive && player2.fireVelocity < MAX_VELOCITY) {
					player2.fireVelocity += VELOCITY_INC;
				} else {
					viewportCamera.transform.Translate (new Vector2 (-1f * SCROLL_INC, 0));
				}
			} else {

				if (player1.isControlActive && player1.fireVelocity < MAX_VELOCITY) {
					player1.fireVelocity += VELOCITY_INC;
				} else {
					viewportCamera.transform.Translate (new Vector2 (-1f * SCROLL_INC, 0));
				}
			}
		} else if (Input.GetKey (KeyCode.RightArrow)) {

			if (!isPlayer1) {
				if (player2.isControlActive && player2.fireVelocity > MIN_VELOCITY) {
					player2.fireVelocity -= VELOCITY_INC;
				} else {
					viewportCamera.transform.Translate (new Vector2 (SCROLL_INC, 0));
				}
			} else {
				
				if (player1.isControlActive && player1.fireVelocity < MAX_VELOCITY * 1f) {
					player1.fireVelocity += VELOCITY_INC;
				} else {
					viewportCamera.transform.Translate (new Vector2 (SCROLL_INC, 0));
				}
			}

			if (!isPlayer1) {
				if (player2.isControlActive && player2.fireVelocity > MIN_VELOCITY) {
					player2.fireVelocity -= VELOCITY_INC;
				} else {
					viewportCamera.transform.Translate (new Vector2 (SCROLL_INC, 0));
				}
			}
		} else if ( Input.GetKey (KeyCode.UpArrow) ) {

			if (!isPlayer1 && player2.isControlActive && player2.fireAngle > -1f * MAX_ANGLE) {
				player2.fireAngle -= ANGLE_INC;
			} else if (isPlayer1 && player1.isControlActive && player1.fireAngle > -1f * MAX_ANGLE) {
				player1.fireAngle -= ANGLE_INC;
			} else {
				viewportCamera.transform.Translate (new Vector3 (0, -1f * ZOOM_INC_Y, ZOOM_INC_Z));
			}
		} else if ( Input.GetKey (KeyCode.DownArrow) ) {

			if (!isPlayer1 && player2.isControlActive && player2.fireAngle > -1f * MIN_ANGLE) {
				player2.fireAngle += ANGLE_INC;
			} else if (isPlayer1 && player1.isControlActive && player1.fireAngle < -1f * MIN_ANGLE) {
				player1.fireAngle += ANGLE_INC;
			} else if (!player1.isControlActive && !player2.isControlActive) {
				viewportCamera.transform.Translate (new Vector3 (0, ZOOM_INC_Y, -1f * ZOOM_INC_Z));
			}
		}

		// Toggle calculator state
		if (Input.GetKeyDown (KeyCode.Return)) {

			if (!isPlayer1) {
				int step = player2.CURRENT_STEP++;
				if (step == 6) {
					player2.CURRENT_STEP = 1;
				}
				
				Debug.Log ("Enter key pressed " + player2.CURRENT_STEP);
			} else {
				int step = player1.CURRENT_STEP++;
				if (step == 6) {
					player1.CURRENT_STEP = 1;
				}

			}
		}

		if ( Input.GetMouseButtonUp (0) && ((!isPlayer1 && !player2.isMousedOver) || (isPlayer1 && !player1.isMousedOver)) ) {

			Debug.Log ("Global click");

			player1Capsule.renderer.material.SetColor ("_Color", Color.gray);
			player2Capsule.renderer.material.SetColor ("_Color", Color.gray);
			player1.isControlActive = false;
			player2.isControlActive = false;

			// turn off particles
			player1.sightLineParticleSystem.GetComponent<ParticleSystem> ().renderer.enabled = false;
			player2.sightLineParticleSystem.GetComponent<ParticleSystem> ().renderer.enabled = false;
		
		}
	}
}