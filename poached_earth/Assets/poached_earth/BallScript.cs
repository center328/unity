using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
	public PlayerController playerController;

	void OnCollisionEnter( Collision coll )
	{
		playerController.onBallCollided ( coll );
		GameObject.Destroy (gameObject);
	}
}