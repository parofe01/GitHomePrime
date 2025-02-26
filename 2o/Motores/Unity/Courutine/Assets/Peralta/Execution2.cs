using UnityEngine;
using System.Collections;

public class Execution2 : MonoBehaviour {

	
	// Use this for initialization
	void Start () {
		Debug.Log("Hola comenzamos");

		SayHello ();

		Debug.Log("Hola de nuevo");
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Frame: " + Time.frameCount);
		
	}


	void SayHello()
	{
		Debug.Log ("Hola desde la funcion");
	}

}
