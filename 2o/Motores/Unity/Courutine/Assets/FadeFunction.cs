using UnityEngine;
using System.Collections;

public class FadeFunction : MonoBehaviour {

	// Use this for initialization
	void Start () {
	Fade ();

	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Frame: " + Time.frameCount);

	}


	void Fade() {
		for (float f = 1f; f >= 0; f -= 0.1f) {
			Color c = GetComponent<Renderer>().material.color;
			c.a = f;
			GetComponent<Renderer>().material.color = c;
			Debug.Log("Alpha: " + c.a);
		}
	}

}
