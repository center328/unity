using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool isMousedOver = false;
	public bool isControlActive = false;
	public float fireAngle = 0;

	public GameObject sightLineParticleSystem;

	public GameObject ballPrefab;
	private GameObject realBall;
	private bool isBallThrown;
	private float power;
	
	public void onBallCollided( Collision collision )
	{
		GameObject.Destroy (realBall);

		if (collision.gameObject.tag == "enemy") {
			Debug.Log ("well done!");
		} else {
			Debug.Log ("try again");
		}
	}

	// Use this for initialization
	void Start () {
		sightLineParticleSystem = gameObject.transform.GetChild (0).gameObject;
		sightLineParticleSystem.GetComponent<ParticleSystem> ().renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion angles = Quaternion.Euler (new Vector3 (fireAngle, 0, 0));
		sightLineParticleSystem.transform.rotation = angles;

		if (Input.GetKeyDown (KeyCode.Space)) {
			
			if (isControlActive) {
				fireBall();
			}
		}
	}

	private void createBall()
	{
		realBall = GameObject.Instantiate (ballPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
	}

	private void fireBall () {
		createBall (); //test
		realBall.rigidbody.useGravity = true;
		realBall.rigidbody.velocity = new Vector3 (0, 20.0f, 20.0f); // Never exceed like 30f
	}

	private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos) {
		return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y))*power;
	}

	// Click handler
	void OnMouseUp() {
		Debug.Log ("Player mouse up");
		gameObject.renderer.material.SetColor ("_Color", Color.red);
		isControlActive = true;
		sightLineParticleSystem.GetComponent<ParticleSystem> ().renderer.enabled = true;
	}

	// Mouse over handler
	void OnMouseEnter() {
		Debug.Log ("Player mouse over");
		isMousedOver = true;
	}

	// Mouse out handler
	void OnMouseExit() {
		Debug.Log ("Player mouse out");
		isMousedOver = false;
	}
}
