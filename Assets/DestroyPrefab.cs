using UnityEngine;
using System.Collections;

public class DestroyPrefab : MonoBehaviour {

	public GameObject MainCamera;
	private float difference;


	// Use this for initialization
	void Start () {

		this.MainCamera = GameObject.Find ("MainCamera");


		
	}
	
	// Update is called once per frame
	void Update () {
		this.difference = this.transform.position.z - MainCamera.transform.position.z;
		if (this.difference <= 0) {
			Destroy (gameObject);
		}

		
	}
}