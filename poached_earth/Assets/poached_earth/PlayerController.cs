using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool isMousedOver = false;
	public bool isControlActive = false;

	public float fireAngle;
	public float fireVelocity;

	public GameObject sightLineParticleSystem;

	public GUIText angleDisplay;
	public GUIText velocityDisplay;
	public GUIText resultDisplay;
	
	public LevelController levelController;

	public GameObject ballPrefab;
	GameObject realBall;
	bool isBallThrown;
	float power;

	public void onBallCollided( Collision collision )
	{
		if (collision.gameObject.tag == "enemy") {
			Debug.Log ("WELL DONE! :-)");
			resultDisplay.text = "NICE SHOT! :-)";

		} else {
			Debug.Log ("TRY AGAIN");
			resultDisplay.text = "Try again.";
		}
	}

	// Use this for initialization
	void Start () {
		sightLineParticleSystem = gameObject.transform.GetChild (0).gameObject;
		sightLineParticleSystem.GetComponent<ParticleSystem>().renderer.enabled = false;

		fireAngle = LevelController.MIN_ANGLE;
		fireVelocity = LevelController.MIN_VELOCITY;
		//fireBall();
	}
	
	// Update is called once per frame
	void Update () {
		if (!levelController.isPlayer1) {
			if (fireAngle > -1.0f) {
				fireAngle = -1f * LevelController.MIN_ANGLE;
			}
			angleDisplay.text = "Angle: " + (Mathf.Round (fireAngle * 10f * -1) / 10f) + " deg";
		}
		velocityDisplay.text = "Velocity: " + (Mathf.Round(fireVelocity * 100f) / 100f) + " m/s";

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
		
		Debug.Log ("fireAngle: " + fireAngle);
		Debug.Log ("fireVelocity: " + fireVelocity);

		float positiveAngle = fireAngle;
		if (!levelController.isPlayer1) {
			positiveAngle = fireAngle * -1f;
		}

		float forwardVelocity = fireVelocity;

		float xVel = Mathf.Cos ((positiveAngle/360f) * 2f * Mathf.PI); // minus one to make positive;
		float yVel = Mathf.Sin ((positiveAngle/360f) * 2f * Mathf.PI);

		Debug.Log ("positiveAngle: " + positiveAngle);
		Debug.Log ("forwardVelocity: " + forwardVelocity);

		Debug.Log ("xVel: " + xVel);
		Debug.Log ("yVel: " + yVel);

		Debug.Log ("X velocity: " + (forwardVelocity * xVel));
		Debug.Log ("Y velocity: " + (forwardVelocity * yVel));

		if (!levelController.isPlayer1) {
			realBall.rigidbody.velocity = new Vector3 (forwardVelocity * xVel * -1f, forwardVelocity * yVel, 0); // Never exceed like 30f
		} else {
			realBall.rigidbody.velocity = new Vector3 (forwardVelocity * xVel, forwardVelocity * yVel, 0); // Never exceed like 30f
		}
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
