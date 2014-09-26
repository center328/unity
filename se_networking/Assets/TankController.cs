using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {

	GameObject barrel;
	public float angle = 0f;

	void Start ()
	{
		barrel = gameObject.transform.GetChild(0).gameObject;
	}
	
	void Update ()
	{
		Quaternion angles = Quaternion.Euler( new Vector3(angle,0,0) );
		barrel.transform.rotation = angles;
	}
}
