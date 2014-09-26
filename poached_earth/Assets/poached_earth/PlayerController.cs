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

	public Color c1 = Color.yellow;
	public Color c2 = Color.red;
	public int lengthOfLineRenderer = 2;

	private int lineX1 = 0;
	private int lineX2 = 200;
	private int lineY1 = 0;
	private int lineY2 = 200;

	public GUIText calculatorInstructionDisplay;
	public GUIText calculatorAngleDisplay;
	public GUIText calculatorVelocityDisplay;
	public GUIText calculatorDistanceDisplay;
	public GUIText calculatorStepDisplay;

	string angleValueString;
	string velocityValueString;
	const string calcVelText = "Total Velocity will be: ";
	const string calcAngleText = "Angle will be: ";
	const string calcDistanceText = "Distance is: m";

	public int CURRENT_STEP = 1;
	const string step1Text = "Step 1: Calculate the effect of the angle on the \n horizontal, X-direction:";
	const string step2Text = "Step 2: Calculate the effect of the angle on the \n vertical, Y-direction:";
	const string step3Text = "Step 3: Calculate the X-velocity of the throw:";
	const string step4Text = "Step 3: Calculate the X-velocity of the throw:";

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

		sightLineParticleSystem.GetComponent<ParticleSystem> ().startLifetime = 4.0f;

		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer> ();
		lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		lineRenderer.SetColors (c1, c2);
		lineRenderer.SetWidth (0.2f, 0.2f);
		lineRenderer.SetVertexCount (lengthOfLineRenderer);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!levelController.isPlayer1)
		{
			if (fireAngle > -1.0f) {
				fireAngle = -1f * LevelController.MIN_ANGLE;
			}
			angleValueString = (Mathf.Round (fireAngle * 10f * -1) / 10f) + " deg";
			angleDisplay.text = "Angle: " + angleValueString;
		}
		velocityValueString = (Mathf.Round (fireVelocity * 100f) / 100f) + " m/s";
		velocityDisplay.text = "Velocity: " + velocityValueString;

		calculatorVelocityDisplay.text = calcVelText + velocityValueString;
		calculatorAngleDisplay.text = calcAngleText + angleValueString;

		switch (CURRENT_STEP) {
			case 0:
				calculatorStepDisplay.enabled = false;
				calculatorAngleDisplay.enabled = false;
				calculatorVelocityDisplay.enabled = false;
				calculatorDistanceDisplay.enabled = false;
				break;
			case 1:
				calculatorStepDisplay.enabled = true;
				calculatorAngleDisplay.enabled = true;
				calculatorVelocityDisplay.enabled = true;
				calculatorDistanceDisplay.enabled = true;

				calculatorStepDisplay.text = step1Text;
				break;
			case 2:
				calculatorStepDisplay.text = step2Text;
				break;
		}

		//Debug.Log ("startLifetime: " + Mathf.Abs(fireVelocity) / LevelController.MAX_VELOCITY * 8.0f);


		//sightLineParticleSystem.GetComponent<ParticleSystem>().renderer.enabled = false;
		sightLineParticleSystem.GetComponent<ParticleSystem> ().startLifetime = Mathf.Abs(fireVelocity) / LevelController.MAX_VELOCITY * 4.0f;
		//sightLineParticleSystem.GetComponent<ParticleSystem> ().startSpeed = 10;
		//sightLineParticleSystem.GetComponent<ParticleSystem>().renderer.enabled = true;

		Quaternion angles = Quaternion.Euler (new Vector3 (fireAngle, -90, 0));
		sightLineParticleSystem.transform.rotation = angles;

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (isControlActive) {
				fireBall();
			}
		}

		/*LineRenderer lineRenderer = GetComponent<LineRenderer> ();
		int i = 0;
		//while (i < lengthOfLineRenderer) {
			//Vector3 pos1 = new Vector3(x1, y1, 0);
		Debug.Log ("DEBUG: " + gameObject.transform.position);
		if (gameObject.transform.position.x != null) {
			lineRenderer.SetPosition (0, gameObject.transform.position);
		}
		Vector3 pos2 = new Vector3(x2 * Mathf.Abs(fireVelocity) / LevelController.MAX_VELOCITY, y2 * Mathf.Abs(fireVelocity) / LevelController.MAX_VELOCITY, 0);
			//lineRenderer.SetPosition(1, gameObject.transform.position);
		lineRenderer.SetPosition(1, pos2);*/
			//i++;
		//}
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
