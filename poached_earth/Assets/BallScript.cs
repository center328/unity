using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public PlayerController playerController;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnCollisionEnter( Collision coll )
	{
		playerController.onBallCollided ( coll );
	}
}
