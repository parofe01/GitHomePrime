using UnityEngine;
using System.Collections;

public class FadeCourutine : MonoBehaviour {

	// Use this for initialization
	void Start () {
	 	StartCoroutine("Fade");
		//************************
		// StopCoroutine
		//**************************
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Frame: " + Time.frameCount);
		
	}
	
	
	IEnumerator Fade() {
		for (float f = 1f; f >= 0; f -= 0.1f) {
			Color c = GetComponent<Renderer>().material.color;
			c.a = f;
			GetComponent<Renderer>().material.color = c;
			Debug.Log("Alpha: " + c.a);
			yield return null;
			//yield return new WaitForSeconds(0.2f);
		}
	}
}
