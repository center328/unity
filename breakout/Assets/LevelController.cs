using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{
	public GameObject paddle;

	// Use this for initialization
	void Start ()
	{
		Debug.Log( "start" );

//		gameObject.renderer.material.SetColor()
	}

//	void OnGUI()
//	{
//		if( GUI.Button(new Rect(0,0,150,25), "my button") )
//		{
//			Debug.Log( "push button" );
//		}
//	}
	
	// Update is called once per frame
	void Update ()
	{
		if( Input.GetKey( KeyCode.LeftArrow ) )
		{
			paddle.transform.Translate( new Vector3( -.1f, 0 ) );
		}
		else if( Input.GetKey(KeyCode.RightArrow) )
		{
			paddle.transform.Translate( new Vector3( .1f, 0 ) );
		}
	}
}







