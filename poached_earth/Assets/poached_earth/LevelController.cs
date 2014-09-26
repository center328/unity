using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public GameObject paddle;
	public GameObject viewportCamera;

	public GameObject player1Capsule;
	public GameObject player2Capsule;

	public PlayerController activePlayer;

	// Use this for initialization
	void Start () {
//		Debug.Log("start");
	}
	
	// Update is called once per frame
	void Update () {
//		GL.PushMatrix();
//		GL.LoadOrtho();
//		GL.Begin(GL.LINES);
//		GL.Color(Color.red);
//		GL.Vertex(new Vector3(0,0,0));
//		GL.Vertex(new Vector3(10,10,0));
//		GL.End ();
//		GL.PopMatrix();

		if (Input.GetKey (KeyCode.LeftArrow)) {
	
			if (activePlayer.isControlActive) {
				activePlayer.fireVelocity -= 0.1f;
			} else {
				viewportCamera.transform.Translate (new Vector2 (-0.2f, 0));
			}
		} else if (Input.GetKey (KeyCode.RightArrow)) {

			if (activePlayer.isControlActive) {
				activePlayer.fireVelocity += 0.1f;
			} else {
				viewportCamera.transform.Translate (new Vector2 (0.2f, 0));
			}
		} else if ( Input.GetKey (KeyCode.UpArrow) ) {

			if (activePlayer.isControlActive) {
				activePlayer.fireAngle += 0.1f;
			} else {
				viewportCamera.transform.Translate (new Vector3 (0, 0, 0.2f));
			}
		} else if ( Input.GetKey (KeyCode.DownArrow) ) {

			if (activePlayer.isControlActive) {
				activePlayer.fireAngle -= 0.1f;
			} else {
				viewportCamera.transform.Translate (new Vector3 (0, 0, -0.2f));
			}
		}

		if (Input.GetKeyDown (KeyCode.Return)) {

			Debug.Log ("Enter key pressed");
		}

		if (Input.GetMouseButtonUp (0) && !activePlayer.isMousedOver) {

			Debug.Log("Global click");
			player1Capsule.renderer.material.SetColor ("_Color",Color.gray);
			activePlayer.isControlActive = false;
			// turn of particles
			activePlayer.sightLineParticleSystem.GetComponent<ParticleSystem> ().renderer.enabled = false;
		}
	}
}
