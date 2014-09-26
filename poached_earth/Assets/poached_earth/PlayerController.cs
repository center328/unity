using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public string playerName;

	public bool isMousedOver;
	public bool isControlActive;

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
	public GUIText calculatorEquationDisplay;

	string angleValueString;
	string velocityValueString;
	string distanceValueString;
	const string calcVelText = "Total Throw Velocity will be: ";
	const string calcAngleText = "Throw Angle is: ";
	const string calcDistanceText = "Distance is: ";

	public int CURRENT_STEP = 1;
	const string step1Text = "Step 1: Calculate the effect of the angle on the \n horizontal, X-direction:";
	const string step2Text = "Step 2: Calculate the effect of the angle on the \n vertical, Y-direction:";
	const string step3Text = "Step 3: Calculate the X-velocity of the throw:";
	const string step4Text = "Step 4: Calculate the Y-velocity of the throw:";
	const string step5Text = "Step 5: Calculate the Number of Seconds in air";
	const string step6Text = "Step 6: Calculate X-distance ball travels";

	const string step1EqA = " = xEffect = cos(";
	const string step1EqB = " * 2pi / 360)";

	const string step2EqA = " = yEffect = sin(";
	const string step2EqB = " * 2pi / 360)";

	const string step3EqA = " = xVelocity = ";
	const string step3EqB = " * ";

	const string step4EqA = " = yVelocity = ";
	const string step4EqB = " * ";

	const string step5EqA = " = timeInAir = ";
	const string step5EqB = "2 * ( -"; 
	const string step5EqC = " / -9.8m/s/s )";
	
	const string step6EqA = " = xDistance = ";
	//const string step6EqB = " ( xVelocity * timeInAir )";

	float answer1;
	float answer2;
	float answer3;
	float answer4;
	float answer5;
	float answer6;
	
	public void onBallCollided( Collision collision )
	{
		if (collision.gameObject.tag == "enemy") {
			Debug.Log ("WELL DONE! :-)");
			resultDisplay.text = "NICE SHOT! :-)";

		} else {
			Debug.Log ("TRY AGAIN");
			resultDisplay.text = "Try again.";
		}
		levelController.isPlayer1 = !levelController.isPlayer1;
		if (levelController.isPlayer1) {
			levelController.player2Capsule.tag = "enemy";
			levelController.player1Capsule.tag = "Untagged";
			levelController.player1.fireAngle = 45f;
			levelController.player1.fireAngle -= 90f;
			levelController.player1.CURRENT_STEP = 1;
			
			sightLineParticleSystem.transform.rotation = Quaternion.identity;
		} else {
			levelController.player1Capsule.tag = "enemy";
			levelController.player2Capsule.tag = "Untagged";
			levelController.player2.fireAngle = 135f;
			levelController.player2.CURRENT_STEP = 1;
		}

		levelController.player1Capsule.renderer.material.SetColor ("_Color", Color.gray);
		levelController.player2Capsule.renderer.material.SetColor ("_Color", Color.gray);
		levelController.player1.isControlActive = false;
		levelController.player2.isControlActive = false;
		
		// turn off particles
		levelController.player1.sightLineParticleSystem.GetComponent<ParticleSystem> ().renderer.enabled = false;
		levelController.player2.sightLineParticleSystem.GetComponent<ParticleSystem> ().renderer.enabled = false;
	}
	
	// Use this for initialization
	void Start () {
		
		//sightLineParticleSystem = gameObject.transform.GetChild (0).gameObject;
		sightLineParticleSystem.GetComponent<ParticleSystem>().renderer.enabled = false;

		fireAngle = LevelController.DEFAULT_ANGLE_1;
		fireVelocity = LevelController.MIN_VELOCITY;
		//fireBall();

		sightLineParticleSystem.GetComponent<ParticleSystem> ().startLifetime = 4.0f;

		isControlActive = false;

		/*LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer> ();
		lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		lineRenderer.SetColors (c1, c2);
		lineRenderer.SetWidth (0.2f, 0.2f);
		lineRenderer.SetVertexCount (lengthOfLineRenderer);*/
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log ("DEBUG isPlayer1: " + playerName);

		if (levelController.isPlayer1 && playerName == "player2") {
			return;
		}
		
		if (!levelController.isPlayer1 && playerName == "player1") {
			return;
		}
		
		if (!levelController.isPlayer1 ) { // Player 2
			angleValueString = Mathf.Abs(Mathf.Round ((180-fireAngle) * 10f) / 10f) + " deg";
			//Debug.Log (angleValueString);
			angleDisplay.text = "Angle: " + angleValueString;
		} else { // Player 1
			angleValueString = Mathf.Abs(Mathf.Round (fireAngle * 10f) / 10f) + " deg";
			angleDisplay.text = "Angle: " + angleValueString;
		}
		velocityValueString = (Mathf.Round (fireVelocity * 100f) / 100f) + " m/s";
		velocityDisplay.text = "Velocity: " + velocityValueString;

		calculatorVelocityDisplay.text = calcVelText + velocityValueString;
		calculatorAngleDisplay.text = calcAngleText + angleValueString;
		calculatorDistanceDisplay.text = calcDistanceText + levelController.distanceValue + " m";

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

				answer1 = Mathf.Round(Mathf.Cos(fireAngle * 2 * Mathf.PI / 360f) * 100f) / 100f;
				calculatorEquationDisplay.text = answer1 + step1EqA + angleValueString + step1EqB;
				break;
			case 2:
				calculatorStepDisplay.text = step2Text;
			
				answer2 = Mathf.Round(Mathf.Sin(Mathf.Abs(fireAngle) * 2 * Mathf.PI / 360f) * 100f) / 100f;
				calculatorEquationDisplay.text = answer2 + step2EqA + angleValueString + step2EqB;	
				break;
			case 3:
				calculatorStepDisplay.text = step3Text;
			
				answer3 = answer1 * Mathf.Abs(fireVelocity);
				calculatorEquationDisplay.text = answer3 + step3EqA + answer1 + step3EqB + Mathf.Abs(fireVelocity);	
				break;
			case 4:
				calculatorStepDisplay.text = step4Text;
			
				answer4 = answer2 * Mathf.Abs(fireVelocity);
				calculatorEquationDisplay.text = answer4 + step4EqA + answer2 + step4EqB + Mathf.Abs(fireVelocity);	
				break;
			case 5:
				calculatorStepDisplay.text = step5Text;

				answer5 = 2 * ( -answer4 / -9.8f);
				calculatorEquationDisplay.text = answer5 + step5EqA + step5EqB + answer4 + step5EqC;	
				break;

			case 6:
				calculatorStepDisplay.text = step6Text;
			
				answer6 = answer5 * answer3;
				calculatorEquationDisplay.text = answer6 + step6EqA + "( " + answer5 + " * " + answer6 + " )";	

				break;

		}
		
		//Debug.Log ("startLifetime: " + Mathf.Abs(fireVelocity) / LevelController.MAX_VELOCITY * 8.0f);


		//sightLineParticleSystem.GetComponent<ParticleSystem>().renderer.enabled = false;
		sightLineParticleSystem.GetComponent<ParticleSystem> ().startLifetime = Mathf.Abs(fireVelocity) / LevelController.MAX_VELOCITY * 4.0f;
		//sightLineParticleSystem.GetComponent<ParticleSystem> ().startSpeed = 10;
		//sightLineParticleSystem.GetComponent<ParticleSystem>().renderer.enabled = true;
		Quaternion angles;
		if (levelController.isPlayer1) {
			//Debug.Log ("Player 1 angle: " + fireAngle);
			angles = Quaternion.Euler (new Vector3 (fireAngle, 90, 0));
		} else {
			angles = Quaternion.Euler (new Vector3 (-fireAngle, 90, 0));
		}
		sightLineParticleSystem.transform.rotation = angles;

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (isControlActive) {
				fireBall();
			}
		}
	}

	private void createBall()
	{
		if (levelController.isPlayer1) {
			realBall = GameObject.Instantiate (ballPrefab, levelController.player1Capsule.transform.position, Quaternion.identity) as GameObject;
		} else {
			realBall = GameObject.Instantiate (ballPrefab, levelController.player2Capsule.transform.position, Quaternion.identity) as GameObject;
		}
		realBall.GetComponent<BallScript>().playerController = this;
	}

	private void fireBall () {
		createBall(); //test
		realBall.rigidbody.useGravity = true;
		
		Debug.Log ("fireAngle: " + fireAngle);
		Debug.Log ("fireVelocity: " + fireVelocity);

		Debug.Log ("isPlayer1: " + levelController.isPlayer1);

		float positiveAngle = fireAngle * -1f;
		if (!levelController.isPlayer1) {
			positiveAngle = fireAngle * 1f;
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

		//if (!levelController.isPlayer1) {
		//realBall.rigidbody.velocity = new Vector3 (forwardVelocity * xVel * -1f, forwardVelocity * yVel, 0); // Never exceed like 30f
		//} else {
		realBall.rigidbody.velocity = new Vector3 (forwardVelocity * xVel, forwardVelocity * yVel, 0); // Never exceed like 30f
		//}
	}

	// Click handler
	void OnMouseUp() {
		//Debug.Log ("Player mouse up");
		isControlActive = true;
		gameObject.renderer.material.SetColor ("_Color", Color.red);
		sightLineParticleSystem.GetComponent<ParticleSystem>().renderer.enabled = true;
	}

	// Mouse over handler
	void OnMouseEnter() {
		//Debug.Log ("Player mouse over");
		isMousedOver = true;
	}

	// Mouse out handler
	void OnMouseExit() {
		//Debug.Log ("Player mouse out");
		isMousedOver = false;
	}
}
