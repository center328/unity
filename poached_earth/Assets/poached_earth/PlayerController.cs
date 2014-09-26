using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool isMousedOver = false;
	public bool isControlActive = false;

	public float fireAngle = 0f;
	public float fireVelocity = 0f;

	public GameObject sightLineParticleSystem;

	public GUIText angleDisplay;
	public GUIText velocityDisplay;

	public GameObject ballPrefab;
	GameObject realBall;
	bool isBallThrown;
	float power;
	
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
		sightLineParticleSystem.GetComponent<ParticleSystem>().renderer.enabled = false;

		//fireBall();
	}
	
	// Update is called once per frame
	void Update () {
		angleDisplay.text = "Angle: " + Mathf.Round(fireAngle);
		velocityDisplay.text = "Velocity: " + Mathf.Round(fireVelocity);

		Quaternion angles = Quaternion.Euler (new Vector3 (fireAngle, -90, 0));
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
		realBall.GetComponent<BallScript>().playerController = this;
	}

	private void fireBall () {
		createBall(); //test
		realBall.rigidbody.useGravity = true;
		realBall.rigidbody.velocity = new Vector3(-10f, 20f, 0); // Never exceed like 30f
	}

	// Click handler
	void OnMouseUp() {
		Debug.Log ("Player mouse up");
		isControlActive = true;
		gameObject.renderer.material.SetColor ("_Color", Color.red);
		sightLineParticleSystem.GetComponent<ParticleSystem>().renderer.enabled = true;
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
