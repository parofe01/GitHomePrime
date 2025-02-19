using UnityEngine;
using System.Collections;

public class Execution3 : MonoBehaviour {
	/////*************************
	//private float pp;
	/////*************************

	// Use this for initialization
	void Start () {
		Debug.Log("Hola comenzamos");
		
		SayHello ();
		/////*************************
		//StartCoroutine (HelloCourutine(pp));
		/////*************************
		StartCoroutine ("HelloCourutine");

		Debug.Log("Hola de nuevo");
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Frame: " + Time.frameCount);
		
	}

	/////*************************
	// VARIOS WAITFOR
	/////*************************
	IEnumerator HelloCourutine()
	{
		yield return 0;

		Debug.Log ("Hola desde la courutina");
	}
	
	void SayHello()
	{
		Debug.Log ("Hola desde la funcion");
	}

}
