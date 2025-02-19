using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public float smoothing = 1f;
	public Transform target;
	
	
	void Start ()
	{
		StartCoroutine(MyCoroutine(target));
	}

	void Update()
	{



	}
	
	IEnumerator MyCoroutine (Transform target)
	{

		// PORQUE NO IF?
		while(Vector3.Distance(transform.position, target.position) > 0.05f)
		{
			transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
			Debug.Log ("Frame: " + Time.frameCount);
			yield return null;
		}
		
		print("Reached the target.");
		
		yield return new WaitForSeconds(3f);
		
		print("MyCoroutine is now finished.");
	}
}
